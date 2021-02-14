using Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace Identity
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {

      services.AddDbContext<AppDbContext>(config =>
      {
        config.UseInMemoryDatabase("InMemoryDb");
      });

      services.AddIdentity<IdentityUser, IdentityRole>(config =>
      {
        config.Password.RequiredLength = 4;
        config.Password.RequireDigit = false;
        config.Password.RequireUppercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.SignIn.RequireConfirmedEmail = true;
      })
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();

      services.ConfigureApplicationCookie(config =>
      {
        config.Cookie.Name = "Identity.Cookie";
        config.LoginPath = "/Home/Login";
      });

      services.AddAuthorization(config =>
      {
        var defaultAuthBuilder = new AuthorizationPolicyBuilder();
        var defaultAuthPolicy = defaultAuthBuilder
          .RequireAuthenticatedUser()
          .Build();
        config.DefaultPolicy = defaultAuthPolicy;
      });

      services.AddMailKit(config => config.UseMailKit(_configuration.GetSection("Email").Get<MailKitOptions>()));

      services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}
