using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForgetMeNot.Api.Router
{
    public class UtilRouter : RouterBase
    {
        public UtilRouter()
        {
            UrlFragment = "util";
            SwaggerGroup = "Util";
        }

        public override void AddRoutes(WebApplication app)
        {

            app.MapGet($"/{UrlFragment}/version",
            [AllowAnonymous]
            [ProducesResponseType(typeof(string), 200)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            () =>
            {
                try
                {
                    var version = "0.0.8";
                    return Results.Ok(version);
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
