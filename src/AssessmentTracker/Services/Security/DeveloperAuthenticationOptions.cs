using Microsoft.AspNet.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AssessmentTracker.Services.Security
{
    public class DeveloperAuthenticationOptions : AuthenticationOptions
    {
        public DeveloperAuthenticationOptions()
        {
            this.AuthenticationScheme = DeveloperSecurityExtensions.AuthenticationType;
            this.AutomaticAuthenticate = true;
            this.AutomaticChallenge = true;
        }
    }
}
