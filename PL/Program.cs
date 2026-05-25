using BLL.AbstractServices;
using BLL.ImplementationService;
using DAL.Data;
using DAL.Models.Users;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            builder.Services.AddControllersWithViews();


            //DbContext
            builder.Services.AddDbContext<TabibyDbContext>(
                options => {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("TabibyDbContext"));
                }
            );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    //options.Password.RequireUppercase = true;//by default is true
                    //options.Password.RequireLowercase = true;//by default is true
                    //options.User.RequireUniqueEmail = true;//by default is true
                })
                .AddEntityFrameworkStores<TabibyDbContext>()
                .AddDefaultTokenProviders();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
