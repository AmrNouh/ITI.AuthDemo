using ITI.AuthDemo.Client;
using ITI.AuthDemo.Client.Protos;
using ITI.AuthDemo.Client.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

var _configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IApiKeyProviderService, ApiKeyProviderService>();

builder.Services.AddGrpcClient<greaterService.greaterServiceClient>(options =>
{
    var address = _configuration.GetValue<string>(Consts.GrpcServiceAddressSettingName);
    options.Address = new Uri(address);
}).AddCallCredentials((context, metadata, serviceProvider) =>
{
    var apiKeyProvider = serviceProvider.GetRequiredService<IApiKeyProviderService>();
    var apiKey = apiKeyProvider.GetApiKey();
    metadata.Add(Consts.ApiKeyHeaderName, apiKey);
    return Task.CompletedTask;
});

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/Authenticate", (greaterService.greaterServiceClient _client) => 
{
    var request = new GreatRequest { Name = $"World" };
    var response = _client.Great(request);
    return Results.Ok(response.Message);
});


app.UseRouting();

app.Run();