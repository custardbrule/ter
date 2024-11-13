using API.SSO.Infras;
using API.SSO.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureService(builder.Configuration);

builder.Services.AddCors(c =>
{
    c.DefaultPolicyName = "Default";
    c.AddDefaultPolicy(options =>
    {
        options
         .AllowAnyMethod()
         .AllowAnyHeader()
         .WithOrigins(builder.Configuration.GetRequiredSection("Cors:AllowDomains").Get<string[]>()!);
    });
});

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<AppExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
