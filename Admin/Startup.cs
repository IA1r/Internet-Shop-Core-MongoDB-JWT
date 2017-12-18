using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Core.Model;
using Admin.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Core.Configuration;
using Core.Const;
using Microsoft.IdentityModel.Tokens;

namespace Admin
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddAuthorization();

			services.AddMvc();

			services.RegisterDependencies();


			services.Configure<MongoSettings>(config =>
			{
				config.ConnectionString = Configuration.GetSection("Mongo:ConnectionString").Value;
				config.DatabaseName = Configuration.GetSection("Mongo:DatabaseName").Value;
			});

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.CookieName = ".User.Session";
				options.IdleTimeout = TimeSpan.FromMinutes(15);
			});
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
				{
					HotModuleReplacement = true
				});
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			var options = new JwtBearerOptions
			{

				TokenValidationParameters = {
				   ValidIssuer = JWTOptionsConst.ISSUER,
				   ValidAudience = JWTOptionsConst.AUDIENCE,
				   IssuerSigningKey = new SymmetricSecurityKey(JWTOptionsConst.GenerateKey()),
				   ValidateIssuerSigningKey = true,
				   ValidateLifetime = true,
				}
			};


			app.UseStaticFiles();

			app.UseJwtBearerAuthentication(options);

			//app.UseIdentity();

			app.UseSession();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				routes.MapRoute(
				name: "initFields",
				template: "{controller=Home}/{action=Index}/{type?}");

				routes.MapRoute(
					name: "searchProduct",
					template: "{controller=Home}/{action=Index}/{id?}/{keyword?}");

				routes.MapSpaFallbackRoute(
					name: "spa-fallback",
					defaults: new { controller = "Home", action = "Index" });
			});

		}
	}
}
