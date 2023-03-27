using ForgetMeNot.Api.Domain;
using ForgetMeNot.Api.Dto;
using ForgetMeNot.Api.Infrastructure;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForgetMeNot.Api.Router
{
    public class BuddyRouter : RouterBase
    {
        public BuddyRouter()
        {
            UrlFragment = "buddy";
            SwaggerGroup = "Buddy";
        }

        public override void AddRoutes(WebApplication app)
        {
            app.MapGet($"/{UrlFragment}",
            [Authorize]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(typeof(List<BuddyDto>), StatusCodes.Status200OK)]
            (ILiteDbContext dbContext, HttpContext context) =>
            {
                try
                {
                    var userId = context.UserId();

                    // Get all the related objects for the logged user
                    var relatedList = dbContext.GetRelatedCollection()
                        .Include("$.Users")
                        .Include("$.Occasions")
                        .Include("$.Occasions[*].ForUser")
                        .Find(r => r.Users.Select(u=>u.Id).Any(i=> i == userId));

                    // Select all the related users
                    var buddies = relatedList
                        .SelectMany(r=>r.Users)
                        .Where(u => u.Id != userId)
                        .DistinctBy(u => u.Id)
                        .Select(u => new BuddyDto(u))
                        .ToList();

                    // Assign the occasion collection to the right user
                    if (relatedList?.Any() ?? false)
                    {
                        // Loop the related collection
                        foreach (var related in relatedList)
                        {
                            if (related.Users?.Any(u => u.Id != userId) ?? false)
                            {
                                // Loop the users that are not the logged in user (Loop buddies)
                                foreach (var user in related.Users.Where(u => u.Id != userId))
                                {
                                    // Look for all the occasions where all the users are involved (ForUser == null)
                                    // or the buddy is the recipient of the occassion
                                    var occasions = related
                                        .Occasions
                                        .Where(o => o.ForUser == null || o.ForUser.Id == user.Id);

                                    if (occasions?.Any() ?? false)
                                    {
                                        var buddy = buddies.Single(b => b.UserId == user.Id);

                                        foreach (var occasion in occasions)
                                        {
                                            buddy.Occasions.Add(new OccasionDto(occasion));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return Results.Ok(buddies);

                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(new[] { SwaggerGroup });

            app.MapPost($"/{UrlFragment}/invite",
            [Authorize]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
            (ILiteDbContext dbContext, HttpContext context, InviteCreateRequest request) =>
            {
                try
                {
                    var userId = context.UserId();
                    var user = dbContext.GetUserCollection().FindById(userId);

                    var invite = new Invite
                    {
                        Status = InvitationStatus.Waiting,
                        CreationDate = DateTime.Now,
                        CreatedByUser = user,
                        InvitedUserName = request.InvitedUserName,
                        InvitedUserCustomMessage= request.InvitedUserCustomMessage
                    };

                    dbContext.GetInviteCollection().Insert(invite);

                    return Results.Ok(invite.Id);
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