using Microsoft.AspNet.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AssessmentTracker.Services.Security
{
    public static class DeveloperSecurityExtensions
    {
        public static string UserName = "Developer";

        public static string AuthenticationType = "DevelopmentUseOnly";

        public static IApplicationBuilder UseDeveloperSecurity(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DeveloperSecurityMiddleware>(new DeveloperAuthenticationOptions());
        }
    }
}
