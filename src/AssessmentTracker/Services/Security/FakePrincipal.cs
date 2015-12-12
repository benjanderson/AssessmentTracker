using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AssessmentTracker.Services.Security
{
    public class FakePrincipal : ClaimsPrincipal
    {
        public FakePrincipal(string userName) : base(new FakeIdentity(userName))
        {
        }
    }
}
