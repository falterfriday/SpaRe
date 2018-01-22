using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SpaceRemastered.Data;

namespace SpaceRemastered
{
	public class Startup
	{
		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _env;

		public Startup(IConfiguration config, IHostingEnvironment env)
		{
			_config = config;
			_env = env;
		}



		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<SpaceContext>(cfg =>
			{
				cfg.UseSqlServer(_config.GetConnectionString("SpaceRemasteredConnectionString"));
			});


			services.AddScoped<ISpaceRepository, SpaceRepository>();


			services.AddMvc(opt =>
			{
				if (_env.IsProduction() && _config["DisableSSL"] != "true")
				{
					opt.Filters.Add(new RequireHttpsAttribute());
				}
			})
			.AddJsonOptions
			(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (!env.IsProduction())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();

			//TODO: Implement Identity
			//app.UseAuthentication(); 

			app.UseMvc(cfg =>
			{
				cfg.MapRoute(
					"Default", "{controller}/{action}/{id?}",
					new { controller = "App", action = "Index" });
			});

			if (env.IsDevelopment())
			{
				//TODO: Implement DB seeder on initial build
				//using (var scope = app.ApplicationServices.CreateScope())
				//{
				//	var seeder = scope.ServiceProvider.GetService<SpaceService>();
				//	seeder.Seed().Wait();
				//}
			}
		}
	}
}
