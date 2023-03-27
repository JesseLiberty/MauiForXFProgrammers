namespace ForgetMeNot.Api.Router
{
    public abstract class RouterBase
    {
        public string UrlFragment;
        public string SwaggerGroup;

        protected ILogger Logger;

        public virtual void AddRoutes(WebApplication app)
        {
        }
    }
}
