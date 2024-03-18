
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Extensions;
using Humanizer;
using Microsoft.AspNetCore.OData;
using MoviesAPI.Context;
using MoviesAPI.Repository;
using MoviesAPI.Repository.Interface;
using MoviesAPI.Service.Interface;
using MoviesAPI.Service;

namespace MoviesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            // Add services to the container.

            builder.Services.AddControllers().AddOData(option => { 
                option.Select().Expand().Filter().OrderBy().SetMaxTop(100).Count();
            });
            

            builder.Services.AddDbContext<MoviesContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn"))
);

            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(MyAllowSpecificOrigins);

            //app.UseHttpsRedirection();

            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
