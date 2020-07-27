using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace SampleAuthIssuer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
               .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
               .AddInMemoryClients(IdentityServerConfig.Clients)
               .AddTestUsers(IdentityServerConfig.TestUsers)
               .AddDeveloperSigningCredential();

            IdentityModelEventSource.ShowPII = true; // For testing only!!

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ApiName = "foo-api";
                });

            services.AddAuthorization();

            // Just for testing!
            services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFileServer();
            app.UseEndpoints(e => e.MapControllers().RequireAuthorization());
        }
    }
}
