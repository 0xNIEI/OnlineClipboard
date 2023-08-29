using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using OnlineClipboard.Data;

namespace OnlineClipboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddRazorPages();
            builder.Logging.AddConsole();
            builder.Services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.Name = "__Secure-Antiforgery";
            });
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("localDb")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/Exception");
                app.UseStatusCodePagesWithReExecute("/Error/Status/{0}", "?code={0}");
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Error/ExceptionDev");
                app.UseStatusCodePagesWithReExecute("/Error/StatusDev", "?code={0}");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "changeTheme",
            pattern: "changeTheme/",
            defaults: new { controller = "Home", action = "ChangeTheme" });

            app.MapControllerRoute(
                name: "customRoute",
                pattern: "{id}",
                defaults: new { controller = "Entries", action = "Details" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Entries}/{action=Create}");

            

            app.Use(async (context, next) =>
            {
                if (!context.Response.Headers.Any(x => x.Key == "Content-Security-Policy"))
                {
                    context.Response.Headers.Add("Content-Security-Policy", "default-src 'none'; font-src 'self'; img-src data: w3.org/svg/2000 'self'; object-src 'none'; script-src 'self'; style-src 'self'; connect-src 'self'; base-uri 'none'; form-action 'self'; frame-ancestors 'none';");
                    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                    context.Response.Headers.Add("X-Frame-Options", "DENY");
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                }                

                await next();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.Run();
        }
    }
}