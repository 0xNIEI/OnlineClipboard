using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OnlineClipboard.Data;
using System.Text;

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
                name: "Error/Exception",
                pattern: "Error/Exception",
                defaults: new { controller = "Error", action = "Exception" });
            
            app.MapControllerRoute(
                name: "Error/ExceptionDev",
                pattern: "Error/ExceptionDev",
                defaults: new { controller = "Error", action = "ExceptionDev" });
            
            app.MapControllerRoute(
                name: "Error/Status",
                pattern: "Error/Status",
                defaults: new { controller = "Error", action = "Status" });
            
            app.MapControllerRoute(
                name: "Error/StatusDev",
                pattern: "Error/StatusDev",
                defaults: new { controller = "Error", action = "StatusDev" });


            app.MapControllerRoute(
                name: "changeTheme",
                pattern: "changeTheme",
                defaults: new { controller = "Application", action = "ChangeTheme" });

            app.MapControllerRoute(
                name: "customRoute",
                pattern: "{id}",
                defaults: new { controller = "Entries", action = "Details" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Entries}/{action=Create}");

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Content-Security-Policy", "default-src 'none'; font-src 'self' https://niei.net; img-src data: w3.org/svg/2000 'self'; object-src 'none'; script-src 'self' https://niei.net; style-src 'self' https://niei.net; connect-src 'self'; base-uri 'none'; form-action 'self'; frame-ancestors 'none';");
                context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Append("X-Frame-Options", "DENY");
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
                context.Response.Headers.Append("Permissions-Policy", "accelerometer=(), autoplay=(), display-capture=(), camera=(), geolocation=(self), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
                context.Response.Headers.Append("Cross-Origin-Embedder-Policy", "(require-corp); report-to=\"default\"");
                context.Response.Headers.Append("Cross-Origin-Opener-Policy", "(same-origin); report-to=\"default\"");
                context.Response.Headers.Append("Cross-Origin-Resource-Policy", "(same-site)");

                const string HeaderKeyName = "CF-IPCountry";
                context.Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
                var country = headerValue.ToString() == "" ? "Development" : headerValue.ToString();
                if (country != "CH" && country != "Development")
                {
                    context.Response.StatusCode = 451;
                    var bytes = Encoding.UTF8.GetBytes("HTTP 451 | Unavailable For Legal Reasons");

                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                    return;
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