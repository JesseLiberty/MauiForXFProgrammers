using ForgetMeNot.Api.Domain;
using LiteDB;
using Microsoft.Extensions.Options;

namespace ForgetMeNot.Api.Infrastructure
{
    public class LiteDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public ILiteCollection<User> GetUserCollection()
        {
            return Database.GetCollection<User>();
        }

        public ILiteCollection<Invite> GetInviteCollection()
        {
            return Database.GetCollection<Invite>();
        }

        public ILiteCollection<Occasion> GetOccasionCollection()
        {
            return Database.GetCollection<Occasion>();
        }

        public ILiteCollection<Related> GetRelatedCollection()
        {
            return Database.GetCollection<Related>();
        }

        public LiteDbContext(IOptions<LiteDbOptions> options)
        {
            Database = new LiteDatabase(options.Value.DatabaseLocation);

            BsonMapper.Global.Entity<Related>()
                .DbRef(x => x.Users, "User");

            BsonMapper.Global.Entity<Occasion>()
                .DbRef(x => x.ForUser, "User");

            BsonMapper.Global.Entity<Invite>()
                .DbRef(x => x.CreatedByUser, "User")
                .DbRef(x => x.AcceptedByUser, "User");

            GetUserCollection().EnsureIndex(c => c.Email);

            CreateMockupData();
        }

        void CreateMockupData()
        {
            var hashingManager = new HashingManager();

            var userCollection = GetUserCollection();

            if (userCollection.Count() == 0)
            {
                // Users
                var rodrigo = new User { Email = "rodrigomjuarez@gmail.com", FullName = "Rodrigo Juarez", HashedPassword = hashingManager.HashToString("123456"), Role = Roles.Admin, IsEmailConfirmed = true, Preferences = new List<UserPreference>() };
                rodrigo.Preferences = GetBasePreferences();
                userCollection.Insert(rodrigo);

                var jesse = new User { Email = "jesseliberty@gmail.com", FullName = "Jesse Liberty", HashedPassword = hashingManager.HashToString("123456"), Role = Roles.Admin, IsEmailConfirmed = true };
                jesse.Preferences = GetBasePreferences();
                userCollection.Insert(jesse);

                // Related
                var related = new Related
                {
                    Since = DateTime.Now,
                    RelatedDescription = "Friends",
                    Users = new List<User> { rodrigo, jesse },
                    Occasions = new List<Occasion>
                    {
                        new Occasion
                        {
                            OccasionName = "Known since",
                            Date = new DateTime(2018,5,1),
                            NumDaysToNotify = 7
                        },
                        new Occasion
                        {
                            ForUser = jesse,
                            OccasionName = "Jesse's Birthday",
                            Date = new DateTime(2070,1,1),
                            NumDaysToNotify = 7
                        },
                        new Occasion
                        {
                            ForUser = rodrigo,
                            OccasionName = "Rodrigo's Birthday",
                            Date = new DateTime(2070,12,31),
                            NumDaysToNotify = 7
                        }
                    }
                };

                var relatedCollection = GetRelatedCollection();

                relatedCollection.Insert(related);
            }

        }
        public List<UserPreference> GetBasePreferences()
        {
            var basePreferences = new List<UserPreference>();

            var preferencesNames = new[] { "Shirt size", "Pants size", "Shoe size", "Favorite books", "Book genres", "TV Shows",
                "Songs", "Clothing", "Hobbies", "Interests", "Pets", "Sports (watch)", "Sports (play)", "Political interests",
                 "Local/Town/City", "Farming", "Gardening", "Favorite Magazines", "Favorite Newspapers", "Favorite Podcasts",
                "Social Media", "Web Sites"};

            foreach (var name in preferencesNames)
            {
                basePreferences.Add(new UserPreference { PreferencePrompt = name });
            }

            return basePreferences;
        }
    }

    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }

        // Collections
        ILiteCollection<User> GetUserCollection();
        ILiteCollection<Invite> GetInviteCollection();
        ILiteCollection<Related> GetRelatedCollection();

        // Helpers
        List<UserPreference> GetBasePreferences();
    }

    public class LiteDbOptions
    {
        public string DatabaseLocation { get; set; }
    }
}
