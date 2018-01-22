using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SpaceRemastered
{
	public class Program
	{
		public static void Main(string[] args) => BuildWebHost(args).Run();

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration(SetupConfiguration)
				.UseStartup<Startup>()
				.Build();

		private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
		{
			builder.Sources.Clear();

			builder
				.AddJsonFile("config.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();
		}
	}
}
