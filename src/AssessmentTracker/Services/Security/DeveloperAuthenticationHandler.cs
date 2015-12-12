using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AssessmentTracker.Services.Security
{
    public class DeveloperAuthenticationHandler : AuthenticationHandler<DeveloperAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var user = DeveloperSecurityExtensions.UserName;
            var principal = new FakePrincipal(user);
            var properties = new AuthenticationProperties();
            var result = Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, properties, DeveloperSecurityExtensions.AuthenticationType)));
            return result;
        }
    }
}
