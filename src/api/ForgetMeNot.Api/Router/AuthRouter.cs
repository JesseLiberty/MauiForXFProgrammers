using ForgetMeNot.Api.Domain;
using ForgetMeNot.Api.Dto;
using ForgetMeNot.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ForgetMeNot.Api.Router
{
    public class AuthRouter : RouterBase
    {
        public AuthRouter()
        {
            UrlFragment = "auth";
            SwaggerGroup = "Auth";
        }

        public override void AddRoutes(WebApplication app)
        {

            // TODO: confirm email

            // TODO: reset password

            app.MapPost($"/{UrlFragment}/createaccount",
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(StatusCodes.Status200OK)]
            (ILiteDbContext dbContext, HashingManager hashingManager, AccountCreateRequest accountCreateRequest) =>
            {
                try
                {
                    var existingUser = dbContext.GetUserCollection().FindOne(p => p.Email == accountCreateRequest.Email);

                    if (existingUser != null)
                    {
                        return Results.BadRequest();
                    }

                    var userToAdd = new User
                    {
                        Role = Roles.User,
                        Email = accountCreateRequest.Email,
                        FullName = accountCreateRequest.FullName,
                        HashedPassword = hashingManager.HashToString(accountCreateRequest.PlainPassword),
                        IsEmailConfirmed = true, // TODO: for now, until we implement mail confirmation
                        Preferences = dbContext.GetBasePreferences()
                    };

                    dbContext.GetUserCollection().Insert(userToAdd);

                    return Results.Ok();

                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(new[] { SwaggerGroup });


            app.MapPost($"/{UrlFragment}/gettoken",
            [AllowAnonymous]
            [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            (ILiteDbContext dbContext, HashingManager hashingManager, WebApplicationBuilder builder, LoginRequest user) =>
            {
                try
                {
                    var existingUser = dbContext.GetUserCollection().FindOne(p => p.Email == user.Username);

                    if (existingUser != null && hashingManager.Verify(user.Password, existingUser.HashedPassword))
                    {
                        var issuer = builder.Configuration["Jwt:Issuer"];
                        var audience = builder.Configuration["Jwt:Audience"];
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(issuer: issuer,
                            audience: audience,
                            signingCredentials: credentials,
                            claims: new[] {
                                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                                new Claim(ClaimTypes.Email, existingUser.Email),
                                new Claim(ClaimTypes.Role, existingUser.Role),
                                new Claim(ClaimTypes.Name, existingUser.FullName)
                            });

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var stringToken = tokenHandler.WriteToken(token);

                        return Results.Ok(stringToken);
                    }
                    else
                    {
                        return Results.Unauthorized();
                    }
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(new[] { SwaggerGroup });

        }
    }
}