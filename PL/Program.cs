using BLL.Hubs;
using BLL.Mapper;
using BLL.Services.AbstractServices;
using BLL.Services.AbstractServices.ConsultationModule;
using BLL.Services.AbstractServices.MedicationModule;
using BLL.Services.AbstractServices.Users;
using BLL.Services.ImplementationService;
using BLL.Services.ImplementationService.ConsultationModule;
using BLL.Services.ImplementationService.MedicationModule;
using DAL.Data;
using DAL.Models.Consultation;
using DAL.Models.Users;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<DataSeeder, DataSeeder>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IMedicationService, MedicationService>();
            builder.Services.AddScoped<IConsultationService, ConsultationService>();
            
            builder.Services.AddAutoMapper((x) => { }, typeof(DomainProfile).Assembly);
            builder.Services.AddSignalR();

            

            builder.Services.AddControllersWithViews();


            //DbContext
            builder.Services.AddDbContext<TabibyDbContext>(
                options => {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("TabibyDbContext"));
                }
            );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(
                options =>
                {
                    //options.Password.RequireUppercase = true;//by default is true
                    //options.Password.RequireLowercase = true;//by default is true
                    //options.User.RequireUniqueEmail = true;//by default is true
                })
                .AddRoleManager<RoleManager<IdentityRole<int>>>()
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
            using var scope = app.Services.CreateScope();
            var seed = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            await seed.SeedDatabaseAsync();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<NotificationHub>("/notificationHub");
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
