using ForgetMeNot.Api.Infrastructure;
using ForgetMeNot.Api.Router;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = CreateBuilder(args);

RegisterServices(builder);

var app = BuildApp(builder);

// Add routes
new BuddyRouter().AddRoutes(app);
new UserRouter().AddRoutes(app);
new UtilRouter().AddRoutes(app);
new AuthRouter().AddRoutes(app);
new ProfileRouter().AddRoutes(app);

app.Run();

static WebApplication BuildApp(WebApplicationBuilder builder)
{
    var app = builder.Build();

    // Swagger
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
        "ForgetMeNot API v1");
    });

    // JWT
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseHttpsRedirection();

    return app;
}

static WebApplicationBuilder CreateBuilder(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    // Swagger
    var contact = new OpenApiContact()
    {
        Name = "Jesse Liberty & Rodrigo Juarez",
        Email = "info@forgetmenotglobal.com",
        Url = new Uri("http://forgetmenotglobal.com")
    };

    var license = new OpenApiLicense()
    {
        Name = "Private use",
        Url = new Uri("http://forgetmenotglobal.com")
    };

    var info = new OpenApiInfo()
    {
        Version = "v1",
        Title = "ForgetMeNot API",
        Description = "ForgetMeNot API Description",
        TermsOfService = new Uri("http://forgetmenotglobal.com"),
        Contact = contact,
        License = license
    };

    var securityScheme = new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security",
    };

    var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer" }
        },
        new string[] {}
    }
};

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(o =>
    {
        o.SwaggerDoc("v1", info);
        o.AddSecurityDefinition("Bearer", securityScheme);
        o.AddSecurityRequirement(securityReq);
    });

    // Auth
    builder.Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
    builder.Services.AddAuthorization();

    return builder;
}

static void RegisterServices(WebApplicationBuilder builder)
{

    // LiteDb
    builder.Services.Configure<LiteDbOptions>(builder.Configuration.GetSection("LiteDbOptions"));
    builder.Services.AddSingleton<ILiteDbContext, LiteDbContext>();

    // Services
    builder.Services.AddSingleton(builder);
    builder.Services.AddSingleton<HashingManager>();

}