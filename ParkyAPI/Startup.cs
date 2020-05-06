using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;


namespace ParkyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            services.AddControllers();
            services.AddAutoMapper(typeof(ParkyMappings));
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("ParkyOpenAPISpec",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Parky API National Park",
                        Version = "1",
                        Description = "Parky API NP",
              
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                        }
                    });


                //options.SwaggerDoc("ParkyOpenAPISpecTrails",
                //  new Microsoft.OpenApi.Models.OpenApiInfo()
                //  {
                //      Title = "Parky API Trails",
                //      Version = "1",
                //      Description = "Udemy Parky API Trails",
                 
                //      License = new Microsoft.OpenApi.Models.OpenApiLicense()
                //      {
                //          Name = "MIT License",
                //          Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                //      }
                //  });

                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            });
 


           


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API NP");
                //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
                options.RoutePrefix = "";
            });


            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
