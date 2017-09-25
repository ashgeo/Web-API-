using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using ClipperDBAccess;
using System.Web.Http.Cors;

namespace ClipperCafeERP
{
    enum UserRoles
    {
        Admin,
        Manager,
        User,
        Staff,
    }
    [EnableCors(origins: "http://localhost:3000/", headers: "*", methods: "*")]
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            UserAccount userAccount = GetUser(context.UserName, context.Password);
            if (userAccount != null)
            {
                if (userAccount.UserRole == UserRoles.Admin.ToString())
                {
                    if (context.UserName == userAccount.UserName && context.Password == userAccount.Password)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, UserRoles.Admin.ToString()));
                        identity.AddClaim(new Claim("UserName", userAccount.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Name, userAccount.Employee.FirstName));
                        context.Validated(identity);
                    }
                }
                else if (userAccount.UserRole == UserRoles.Manager.ToString())
                {

                    if (context.UserName == userAccount.UserName && context.Password == userAccount.Password)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, UserRoles.Manager.ToString()));
                        identity.AddClaim(new Claim("UserName", userAccount.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Name, userAccount.Employee.FirstName));
                        context.Validated(identity);
                    }
                }
                else if (userAccount.UserRole == UserRoles.Staff.ToString())
                {



                }
                else if (userAccount.UserRole == UserRoles.User.ToString())
                {

                }
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;
               
            }
        }

        private UserAccount GetUser(string userNameReceived, string passwordReceived)
        {
            var dbcontext = new ClipperDBEntities();
            UserAccount user = null;
            List<UserAccount> userAccounts = dbcontext.UserAccounts.Where(x => x.UserName == userNameReceived && x.Password == passwordReceived).ToList();
            if (userAccounts.Count == 1)
            {
                foreach (UserAccount useraccount in userAccounts)
                {
                    user = useraccount;
                }
            }
            else
            {
                return user;
            }
            return user;
        }


    }
}