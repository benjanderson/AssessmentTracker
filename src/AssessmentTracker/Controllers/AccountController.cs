namespace AssessmentTracker.Controllers
{
	using System.Threading.Tasks;

	using Microsoft.AspNet.Authentication.Cookies;
	using Microsoft.AspNet.Authentication.OpenIdConnect;
	using Microsoft.AspNet.Http.Authentication;
	using Microsoft.AspNet.Mvc;

	public class AccountController : Controller
	{
		public IActionResult SignIn()
		{
			return new ChallengeResult(
					OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = "/" });
		}

		public async Task<IActionResult> SignOut()
		{
			var callbackUrl = this.Url.Action("SignOutCallback", "Account", values: null, protocol: this.Request.Scheme);
			await this.HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			await this.HttpContext.Authentication.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
					new AuthenticationProperties { RedirectUri = callbackUrl });
			return new EmptyResult();
		}

		public IActionResult SignOutCallback()
		{
			if (this.HttpContext.User.Identity.IsAuthenticated)
			{
				// Redirect to home page if the user is authenticated.
				return this.RedirectToAction(nameof(HomeController.Index), "Home");
			}

			return this.View();
		}
	}
}
