using DC8Training.WebAPI.Services;
using QLDP.Models;
using QLDP.Providers;
using QLDP.Repositories;

namespace DC8Training.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularOrigins",
                builder =>
                {
                    builder.WithOrigins(
                                        "http://localhost:4200"
                                        )
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            
            //study:
            //builder.Services.AddScoped
//            builder.Services.AddSession();

            //provider DI
            builder.Services.AddScoped<IDataProvider<Medicine>, SPMedicineProvider>();
            builder.Services.AddScoped<IDataProvider<MedicineCategory>, SPCategoryProvider>();
            builder.Services.AddScoped<IDataProvider<Image>, SPImageProvider>();

            //repo DI
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();

            builder.Services.AddScoped<IMedicineService, MedicineMgtServices>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            /*            builder.Services.AddSession();
                        builder.Services.AddRazorPages();
                        builder.Services.AddServerSideBlazor();*/

          

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

            app.UseCors("AllowAngularOrigins");

            app.Run();
        }
    }
}
