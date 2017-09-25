using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

[assembly: OwinStartup(typeof(ClipperCafeERP.Startup))]

namespace ClipperCafeERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var myProvider = new MyAuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = myProvider
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}

