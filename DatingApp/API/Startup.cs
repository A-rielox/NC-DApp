using API.Extensions;
using API.Middleware;
using Microsoft.OpenApi.Models;

namespace API;

public class Startup
{
    private readonly IConfiguration _config;

    public Startup(IConfiguration config)
    {
        _config = config;
    }

    //public IConfiguration Configuration { get; } NO LA QUIERE

    // This method gets called by the runtime. Use this method to add services to the container.
    // PARA LA DEPENDENCY INJECTION
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAplicationServices(_config); // p' mi metodo de extension ApplicationServiceExtensions

        services.AddControllers();
        services.AddCors();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
        });

        services.AddIdentityServices(_config);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    // PARA LA PIPELINE
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        if (env.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        // debe ir entre UseRouting y Endpoint, y antes de Authorization y UseAuthentication
        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));



        // antes de MapControllers y despues de UseCors
        app.UseAuthentication(); // checa q el token sea valido
        app.UseAuthorization(); // checa lo q tengo permitido hacer de acuerdo a lo q diga mi token
        
        
        
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
