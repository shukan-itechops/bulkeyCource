using bulkey.DataAccess.Data;
using bulkey.DataAccess.Repository;
using bulkey.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using bulkey.Utility;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.ConfigureApplicationCookie(option => {
            option.LoginPath = $"/Identity/Account/Login";
            option.LogoutPath = $"/Identity/Account/Logout";
            option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
        });



        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddRazorPages();
        builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();  

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages(); 

        app.MapControllerRoute(
            name: "default",
            pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}

