using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var hosting = builder.Configuration.GetSection("Hosting");
var ssoReactPort = hosting.GetSection("SsoReact").GetValue<int>("Port");

builder.AddNpmApp("sso-react", "./../sso", scriptName: "dev", args: ["-p", ssoReactPort.ToString()])
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(port: 5001,targetPort: ssoReactPort)
    .WithExternalHttpEndpoints();

builder.Build().Run();
