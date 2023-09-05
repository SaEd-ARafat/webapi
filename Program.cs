using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityServer()
            .AddInMemoryApiResources(new[]
            {
                new ApiResource("your-api-resource", "Your API")
                {
                    Scopes = { "apiScope" }
                }
            })
            .AddInMemoryClients(new[]
            {
                new Client
                {
                    ClientId = "your-client-id",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("your-client-secret".Sha256()) },
                    AllowedScopes = { "apiScope" }
                }
            })
            .AddDeveloperSigningCredential();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://your-identity-server-url";
            options.Audience = "your-api-resource";
        });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseIdentityServer();
//Configure other middleware and routing as needed


app.Run();
