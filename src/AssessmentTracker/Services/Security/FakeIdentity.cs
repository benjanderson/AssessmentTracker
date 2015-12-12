using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AssessmentTracker.Services.Security
{
    public class FakeIdentity : IIdentity
    {
        public FakeIdentity(string userName)
        {
            this.Name = userName;
            this.IsAuthenticated = true;
            this.AuthenticationType = DeveloperSecurityExtensions.AuthenticationType;
        }

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }

    }
}
