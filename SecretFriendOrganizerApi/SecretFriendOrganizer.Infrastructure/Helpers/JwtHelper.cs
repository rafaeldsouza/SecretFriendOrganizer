using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretFriendOrganizer.Infrastructure.Helpers
{
    public static class JwtHelper
    {
        /// <summary>
        /// Extracts the user ID (sub) from a JWT.
        /// </summary>
        /// <param name="accessToken">The JWT access token</param>
        /// <returns>The user ID if found, otherwise null</returns>
        public static string? GetUserIdFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(accessToken))
            {
                var jwtToken = handler.ReadJwtToken(accessToken);

                // Retrieve the "sub" claim from the JWT payload
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                return userId;
            }

            return null; // Invalid token
        }
    }
}
