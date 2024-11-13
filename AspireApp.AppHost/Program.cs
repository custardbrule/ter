using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_SSO>("api-sso")
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(port: 5200, name: "sso-api-http");

var hosting = builder.Configuration.GetSection("Hosting");
var ssoReactPort = hosting.GetSection("SsoReact").GetValue<int>("Port");

builder.AddNpmApp("sso-react", Path.Combine(Directory.GetCurrentDirectory(), "..", "FE", "sso"), scriptName: "dev", args: ["-p", ssoReactPort.ToString()])
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(port: 5001, targetPort: ssoReactPort)
    .WithExternalHttpEndpoints();

builder.Build().Run();
