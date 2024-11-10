﻿using API.SSO.Infras.Context;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace API.SSO.Settings
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var client = await manager.FindByClientIdAsync("sso");
            if (client == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "sso",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "sso client application",
                    RedirectUris =
                {
                    new Uri("https://localhost:3000/callback/login/local")
                },
                    PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:5000/callback/logout/local")
                },
                    Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                },
                    Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
