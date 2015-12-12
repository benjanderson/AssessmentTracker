using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AssessmentTracker.Services.Security
{
    public class DeveloperSecurityMiddleware : AuthenticationMiddleware<DeveloperAuthenticationOptions>
    {
        public DeveloperSecurityMiddleware(
            RequestDelegate next,
            DeveloperAuthenticationOptions options,
            ILoggerFactory loggerFactory,
            IUrlEncoder encoder) : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<DeveloperAuthenticationOptions> CreateHandler()
        {
            return new DeveloperAuthenticationHandler();
        }
    }
}
