using ForgetMeNot.Api.Dto;
using ForgetMeNot.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForgetMeNot.Api.Router
{
    public class ProfileRouter : RouterBase
    {
        public ProfileRouter()
        {
            UrlFragment = "profile";
            SwaggerGroup = "Profile";
        }

        public override void AddRoutes(WebApplication app)
        {

            app.MapGet($"/{UrlFragment}/me",
            [Authorize]
            [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            (ILiteDbContext dbContext, HttpContext context) =>
            {
                try
                {
                    var userId = context.UserId();

                    if (userId == Guid.Empty)
                    {
                        return Results.NotFound();
                    }

                    var user = dbContext.GetUserCollection().FindById(userId);

                    if (user == null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(new UserResponse(user));

                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(new[] { SwaggerGroup });

            app.MapPut($"/{UrlFragment}/me",
            [Authorize]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            (ILiteDbContext dbContext, HttpContext context, ProfileUpdateRequest updatedUser) =>
            {
                try
                {
                    var userId = context.UserId();

                    var userToUpdate = dbContext.GetUserCollection().FindById(userId);

                    if (userToUpdate == null)
                    {
                        return Results.NotFound();
                    }

                    userToUpdate.FullName = updatedUser.FullName;
                    userToUpdate.Preferences = updatedUser.Preferences;

                    dbContext.GetUserCollection().Update(userToUpdate);

                    return Results.Ok();

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