using Domain.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using WebAPI.Managers;
using WebAPI.Managers.Interfaces;
using WebAPI.Mapping;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.Validation;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDBContext>();
            builder.Services.AddControllers();
            builder.Services.AddTransient<IItemRepository, ItemRepository>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IItemService, ItemService>();
            builder.Services.AddAutoMapper(typeof(AppProfile));
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddScoped<IValidator<ItemRequestModel>, ItemValidator>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");
            Directory.CreateDirectory(rootFolder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
