using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Owin;
using Owin;
using SignalRStreaming.BL.Hubs;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models;
using SignalRStreamingJson.Repositorys;
using SignalRStreamingJson.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<MyDbContext>(opt => opt
    .EnableDetailedErrors()
    .UseSqlServer(config.GetConnectionString("SqlServer")), ServiceLifetime.Transient);


builder.Services.AddScoped<IMockRepository, MockRepository>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IMockService, MockService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Beaerer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.ApiKey,
    });


    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        //x.Authority = "Authority URL";

        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetSection("JWTkey").ToString())),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        x.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessTokenResponse = context.HttpContext.Request.Headers["Authorization"].ToString();
                if(String.IsNullOrEmpty(accessTokenResponse))
                    return Task.CompletedTask;

                var accessTokenSkippedFirst = string.Join(string.Empty, accessTokenResponse.Skip(8));
                var finalAccessToken = accessTokenSkippedFirst.Remove(accessTokenSkippedFirst.Length - 1, 1);

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(finalAccessToken) && path.StartsWithSegments("/privatehub"))
                {
                    context.Token = finalAccessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddSignalR(opt =>
{
    opt.EnableDetailedErrors = true;

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllCors", builder =>
    {
        builder

        .WithOrigins("https://localhost:7189", "http://localhost:5189")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .SetIsOriginAllowed(delegate (string requestingOrigin)
        {
            return true;
        }).Build();
    });
});
builder.Services.AddResponseCompression(opt =>
    opt.MimeTypes = ResponseCompressionDefaults
    .MimeTypes
    .Concat(new[] { "application/octet-stream"} )
);




var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<PublicChatHub>("/publichub");
app.MapHub<PrivateChatHub>("/privatehub");



app.MapControllers();

app.Run();
