
namespace AspNetCore3ODataSample.Web
{
	using Microsoft.AspNet.OData.Extensions;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Models;

	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<MovieContext>(opt => opt.UseInMemoryDatabase("MovieList"));
			services.AddOData();
            services.AddControllers(options =>
			{
				options.EnableEndpointRouting = false;
			})
			// NOTE: OData needs the Newtonsoft.Json serializer. It does not work correctly without.
			// TODO: Remove dependency on Newtonsoft.Json and use the new built-in serializer.
			.AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

			app.UseAuthentication();

            app.UseAuthorization();

			var model = EdmModelBuilder.GetEdmModel();

			// NOTE: OData works with ASP.NET Core 3 using the legacy routing system.
			app.UseMvc(builder =>
			{
                builder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

				builder.MapODataServiceRoute("OData", "odata", model);

                builder.MapODataServiceRoute("OData1", "efcore", model);

                builder.MapODataServiceRoute("OData2", "inmem", model);

                builder.MapODataServiceRoute("OData3", "composite", EdmModelBuilder.GetCompositeModel());
            });

			// TODO: OData needs to work using the new ASP.NET Core 3 endpoints routing system.
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

            //    endpoints.MapODataServiceRoute("OData1", "efcore", model);

            //    endpoints.MapODataServiceRoute("OData2", "inmem", model);

            //    endpoints.MapODataServiceRoute("OData3", "composite", EdmModelBuilder.GetCompositeModel());
            //});
        }
    }
}
