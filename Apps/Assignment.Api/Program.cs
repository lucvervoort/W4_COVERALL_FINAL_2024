
/* Docker Commands:

docker build -t nwapi_server -f ./Apps/Assignment.Api/Dockerfile .

docker run --rm -it -p 8080:80 -p 8081:443 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORTS=8081 -e ASPNETCORE_Kestrel__Certificates__Default__Password="ZwarteRidder007" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificates/aspnetapp.pfx -v %USERPROFILE%\.aspnet\https:/https/ nwapi_server
 */

using Assignment.Core.Helpers;
using Assignment.Repository.Context;
using Assignment.Repository.Repositories;
using Assignment.REST.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingConfig));

            builder.Services.AddControllers();

            //Inject Identity to this project
            #region DataBase Configuration

            var cs = builder.Configuration.GetConnectionString("DefaultConnectionString");

            //BD Connection string
            builder.Services.AddDbContext<CarwashContext>(options => options.UseSqlServer(cs));

            #endregion

            #region Repository services

//#if EXAMPLE
            builder.Services.AddScoped<ISortHelper<Assignment.Repository.Models.Klanten>, SortHelper<Assignment.Repository.Models.Klanten>>();
            builder.Services.AddScoped<KlantenRepository>();
//#endif

            #endregion

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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
