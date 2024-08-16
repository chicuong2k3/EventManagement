using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EventManagement.Common.Infrastructure.Authentication
{
    internal sealed class JwtBearerConfigureOptions(IConfiguration configuration) 
        : IConfigureNamedOptions<JwtBearerOptions>
    {
        const string ConfigurationSectionName = "Authentication";
        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
