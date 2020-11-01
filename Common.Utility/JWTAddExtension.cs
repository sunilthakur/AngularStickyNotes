using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Common.Utility
{
    public static class JWTAddExtension
    {
        /// <summary>
        /// Create JWT Token Validation Mehanism
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="builder"></param>
        public static void AddJwtAuthentication(this IServiceCollection services,
            IWebHostEnvironment environment,
            IConfigurationBuilder builder)
        {
            // Hard Coded file should be ENVironment Specific
            IConfigurationRoot _config = builder.Build();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // validate the server
                        ValidateAudience = true, // Validate the recipient of token is authorized to receive
                        ValidateLifetime = true, // Check if token is not expired and the signing key of the issuer is valid 
                        ValidateIssuerSigningKey = true, // Validate signature of the token 

                        //Issuer and audience values are same as defined in generating Token
                        ValidIssuer = _config.GetSection("Jwt")["Issuer"].ToString(), // stored in appsetting file
                        ValidAudience = _config["Jwt:Issuer"], // stored in appsetting file
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])) // stored in appsetting file
                    };
                });
        }
    }
}
