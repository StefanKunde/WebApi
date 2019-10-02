namespace AspNetCore3ODataSample.Web
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Hosting;

	public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureKestrel(options =>
					{
                        // HACK: Allow synchronous IO.
                        // TODO: OData must use asynchronous IO everywhere.
						// NOTE: https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio
                        options.AllowSynchronousIO = true;
					});
					webBuilder.UseStartup<Startup>();
				});
		}
	}
}
