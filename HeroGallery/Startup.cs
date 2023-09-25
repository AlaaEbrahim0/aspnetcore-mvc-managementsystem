

using HeroGallery.Extensions;

namespace HeroGallery;

public class Startup
{
    private IConfiguration _config;


    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

    public Startup(IConfiguration config)
    {
        _config = config;
    }

    public void ConfigureServices(IServiceCollection services)
    {

        ServiceExtensions.ConfigureMvc(services, _config);
        ServiceExtensions.ConfigureAuthorization(services);
        ServiceExtensions.ConfigureIdentity(services);
        ServiceExtensions.ConfigureCors(services);
        ServiceExtensions.ConfigureAuthentication(services, _config);

        services.AddScoped<IHeroRepository, SqlHeroRepository>();

        
        services.ConfigureApplicationCookie(options =>
            options.AccessDeniedPath = new PathString("/Adminstration/AccessDenied"));
        

		services.AddTransient<IEmailSender, EmailSender>();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(5);
        });


        
    }

    // This method gets called by the runtime. Use this method to configure the HTTP reqsuest pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        
        else
            app.UseHsts();

        app.UseExceptionHandler("/Error");

        app.UseStatusCodePagesWithReExecute("/Error/{0}");

        app.UseStaticFiles();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMvcWithDefaultRoute();

    }
}
