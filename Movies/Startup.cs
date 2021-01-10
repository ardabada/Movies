using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Converters;
using Movies.Filters;
using Movies.Models.Configuration;
using Movies.Repositories;
using Movies.Repositories.Impl;
using Movies.Services;
using Movies.Services.Impl;
using System.IO;

namespace Movies
{
    public class Startup
    {
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.Configure<TheMovieDbApiOptions>(Configuration.GetSection("theMovieDbApi"));
            services.Configure<FavoritesOptions>(Configuration.GetSection("favorites"));

            services.AddTransient<TheMovieDbConverter>();
            services.AddTransient<FavoritesOptions>();
            services.AddTransient<IMovieRepository, LocalFileMovieRepository>();
            services.AddTransient<IMovieService, MovieService>();

            services.AddMvc();
            services.AddControllers(options =>
            {
                options.Filters.Add(new ExceptionFilter());
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "__csrfToken";
                options.FormFieldName = "__csrfToken";
                options.HeaderName = "__csrfToken";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
