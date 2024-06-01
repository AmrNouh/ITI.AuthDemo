using ITI.AuthDemo.Server;
using ITI.AuthDemo.Server.Handlers;
using ITI.AuthDemo.Server.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddGrpc();

builder.Services.AddScoped<IApiKeyAuthenticationService, ApiKeyAuthenticationService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Consts.ApiKeySchemeName;
}).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(Consts.ApiKeySchemeName, configureOptions => { });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GreaterAuthService>();

app.Run();
