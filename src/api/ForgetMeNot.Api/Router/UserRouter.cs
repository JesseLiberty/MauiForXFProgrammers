using ForgetMeNot.Api.Dto;
using ForgetMeNot.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForgetMeNot.Api.Router
{
    public class UserRouter : RouterBase
    {
        public UserRouter()
        {
            UrlFragment = "user";
            SwaggerGroup = "User";
        }

        public override void AddRoutes(WebApplication app)
        {
            app.MapGet($"/{UrlFragment}",
            [Authorize(Roles = "admin")]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
            (ILiteDbContext dbContext) =>
            {
                try
                {
                    var collection = dbContext.GetUserCollection().FindAll();
                    var returnedCollection = new List<UserResponse>();
                    foreach (var user in collection)
                    {
                        returnedCollection.Add(new UserResponse(user));
                    }
                    return Results.Ok(returnedCollection);

                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(new[] { SwaggerGroup });

            app.MapGet($"/{UrlFragment}/{{id}}",
            [Authorize(Roles = "admin")]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
            (ILiteDbContext dbContext, int id) =>
            {
                try
                {
                    var user = dbContext.GetUserCollection().FindById(id);

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

            // TODO: change role / check that at least one user still has admin access, can't remove admin from own account
            // TODO: change email registered status

            app.MapPut($"/{UrlFragment}",
            [Authorize(Roles = "admin")]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status200OK)]
            (ILiteDbContext dbContext, UserUpdateRequest request) =>
            {
                try
                {
                    var userToUpdate = dbContext.GetUserCollection().FindById(request.Id);

                    if (userToUpdate == null)
                    {
                        return Results.NotFound();
                    }

                    userToUpdate.Email = request.Email;
                    userToUpdate.FullName = request.FullName;

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