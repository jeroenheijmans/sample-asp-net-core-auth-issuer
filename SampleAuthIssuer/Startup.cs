using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "foo-api";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuers = new[]
                        {
                            "https://localhost:5001",
                            // "https://localhost:5002", // Turn this on to allow it as a valid issuer
                        },
                        NameClaimType = "name",
                        RoleClaimType = "role",
                    };
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
