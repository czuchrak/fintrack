using Fintrack.App;
using Fintrack.App.Functions;
using Fintrack.App.HttpClients;
using Fintrack.App.Mails;
using Fintrack.Database;
using Fintrack.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Fintrack;

public class Startup
{
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _env = env;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Database")));

        services.AddControllersWithViews();
        services.AddHttpClient();
        services.AddHttpClient<INbpHttpClient, NbpHttpClient>();

        services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp"; });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AssemblyClass.Assembly);
            cfg.LicenseKey = Configuration["MediatRLicense"];
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(AssemblyClass.Assembly);

        services.AddSwaggerGen(config => config.CustomSchemaIds(x => x.FullName));

        var firebaseProjectId = Configuration["FirebaseProjectId"];

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://securetoken.google.com/" + firebaseProjectId;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/" + firebaseProjectId,
                    ValidateAudience = true,
                    ValidAudience = firebaseProjectId,
                    ValidateLifetime = true
                };
            });

        services.AddTransient<IMailSender, MailSender>();

        services.AddQuartzJobs(Configuration);

        services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
    }

    public void Configure(IApplicationBuilder app, DatabaseContext context)
    {
        if (_env.IsDevelopment())
        {
            context.CreateDevelopmentDatabase().Wait();
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Add("Cache-Control", "max-age=3000, must-revalidate");
            }
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller}/{action=Index}/{id?}");
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "../../frontend";

            if (_env.IsDevelopment()) spa.UseReactDevelopmentServer("start");
        });
    }
}