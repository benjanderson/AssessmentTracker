namespace AssessmentTracker
{
    using System.Threading.Tasks;

    using AssessmentTracker.DataAccess;

    using Microsoft.AspNet.Authentication;
    using Microsoft.AspNet.Authentication.Cookies;
    using Microsoft.AspNet.Authentication.OpenIdConnect;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.Data.Entity;
    using Microsoft.Dnx.Runtime;

    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Services.Security;

    public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder =
				new ConfigurationBuilder().AddJsonFile("appsettings.json")
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
					.AddEnvironmentVariables();

			if (env.IsDevelopment())
			{
				//builder.AddUserSecrets();
			}

			this.Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			// Add Entity Framework services to the services container.
			services.AddEntityFramework()
					.AddSqlServer()
					.AddDbContext<AssessmentDbContext>(options =>
							options.UseSqlServer(this.Configuration["ConnectionString"]));

			services.AddMvc().AddJsonOptions(
				options =>
					{
						options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					});

			services.AddTransient<IAssessmentDbContext, AssessmentDbContext>();
			services.AddSingleton((serviceProvider) => this.Configuration);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
			loggerFactory.AddDebug(); ;

			app.UseIISPlatformHandler();

			app.UseDeveloperExceptionPage();

			app.UseStaticFiles();

            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthenticate = true;
            });

            if (env.IsDevelopment())
			{
                app.UseDeveloperSecurity();
            }
            else
            {
                app.UseOpenIdConnectAuthentication(options =>
                {
                    options.AutomaticChallenge = true;
                    options.ClientId = this.Configuration["Authentication:AzureAd:ClientId"];
                    options.Authority = this.Configuration["Authentication:AzureAd:AADInstance"] + this.Configuration["Authentication:AzureAd:TenantId"];
                    options.PostLogoutRedirectUri = this.Configuration["Authentication:AzureAd:PostLogoutRedirectUri"];
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });
            }

            app.UseMvc(
				routes =>
					{ routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"); });
		}

		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}

   
}
