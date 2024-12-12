using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Application.Interfaces.Services;
using SecretFriendOrganizer.Infrastructure.Helpers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SecretFriendOrganizer.Infrastructure.Keycloak
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KeycloakService> _logger;

        public KeycloakService(HttpClient httpClient, IConfiguration configuration, ILogger<KeycloakService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            var requiredKeys = new[] { "Keycloak:BaseUrl", "Keycloak:Realm", "Keycloak:ClientId", "Keycloak:ClientSecret" };

            foreach (var key in requiredKeys)
            {
                if (string.IsNullOrEmpty(_configuration[key]))
                {
                    throw new InvalidOperationException($"Keycloak configuration '{key}' is missing or empty.");
                }
            }
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            try
            {
                _logger.LogInformation("Requesting token from Keycloak...");
                var tokenUrl = $"{_configuration["Keycloak:BaseUrl"]}/realms/{_configuration["Keycloak:Realm"]}/protocol/openid-connect/token";

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _configuration["Keycloak:ClientId"]),
                    new KeyValuePair<string, string>("client_secret", _configuration["Keycloak:ClientSecret"])
                });

                var response = await _httpClient.PostAsync(tokenUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get access token. Status: {Status}, Reason: {Reason}",
                        response.StatusCode, response.ReasonPhrase);
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                return tokenData?.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving access token.");
                throw;
            }
        }
      
        public async Task<KeycloakResponse<string>> CreateUserAsync(string username, string email, string password)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    return new KeycloakResponse<string> { Success = false, Message = "Failed to obtain access token." };
                }

                var userUrl = $"{_configuration["Keycloak:BaseUrl"]}/admin/realms/{_configuration["Keycloak:Realm"]}/users";

                var userPayload = new
                {
                    username,
                    email,
                    enabled = true,
                    credentials = new[]
                    {
                        new { type = "password", value = password, temporary = false }
                    }
                };

                var response = await ExecutePostAsync(userUrl, userPayload, token);

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    _logger.LogInformation("Conflict: User {Username} already exists.", username);
                    return new KeycloakResponse<string> { Success = false, Message = "User already exists." };
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to create user. Status: {Status}, Reason: {Reason}",
                        response.StatusCode, response.ReasonPhrase);
                    return new KeycloakResponse<string> { Success = false, Message = "Failed to create user." };
                }

                _logger.LogInformation("User {Username} created successfully.", username);


                var userId = await GetUserIdByEmailAsync(email);
                if (string.IsNullOrEmpty(userId))
                {
                    return new KeycloakResponse<string> { Success = false, Message = "Failed to retrieve user ID." };
                }

                _logger.LogInformation("User {Username} created successfully with ID {UserId}.", username, userId);
                return new KeycloakResponse<string> { Success = true, Message = "User created successfully.", Data = userId };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user.");
                return new KeycloakResponse<string> { Success = false, Message = ex.Message };
            }
        }

        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var userUrl = $"{_configuration["Keycloak:BaseUrl"]}/admin/realms/{_configuration["Keycloak:Realm"]}/users?email={email}";

            var response = await _httpClient.GetAsync(userUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to retrieve user ID. Status: {Status}, Reason: {Reason}", response.StatusCode, response.ReasonPhrase);
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, 
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
                AllowTrailingCommas = true 
            };

            var users = JsonSerializer.Deserialize<List<KeycloakUser>>(responseContent, options);

            return users?.FirstOrDefault()?.Id;
        }

        public async Task<KeycloakResponse> AddUserToGroupAsync(string userId, string groupId)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    return new KeycloakResponse { Success = false, Message = "Failed to obtain access token." };
                }

                var groupUrl = $"{_configuration["Keycloak:BaseUrl"]}/admin/realms/{_configuration["Keycloak:Realm"]}/users/{userId}/groups/{groupId}";

                var response = await ExecutePutAsync(groupUrl, token);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to add user {UserId} to group {GroupId}. Status: {Status}, Reason: {Reason}",
                        userId, groupId, response.StatusCode, response.ReasonPhrase);
                    return new KeycloakResponse { Success = false, Message = "Failed to add user to group." };
                }

                _logger.LogInformation("User {UserId} added to group {GroupId} successfully.", userId, groupId);
                return new KeycloakResponse { Success = true, Message = "User added to group successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user to group.");
                return new KeycloakResponse { Success = false, Message = ex.Message };
            }
        }

        private async Task<HttpResponseMessage> ExecutePostAsync(string url, object payload, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }

        private async Task<HttpResponseMessage> ExecutePutAsync(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.PutAsync(url, null);
        }

        public async Task<KeycloakResponse<KeycloakToken>> AuthenticateAsync(string username, string password)
        {
            try
            {
                var tokenUrl = $"{_configuration["Keycloak:BaseUrl"]}/realms/{_configuration["Keycloak:Realm"]}/protocol/openid-connect/token";

                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _configuration["Keycloak:ClientId"]),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("client_secret", _configuration["Keycloak:ClientSecret"])
        });                
                var response = await _httpClient.PostAsync(tokenUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Authentication failed for user {Username}. Status: {Status}, Reason: {Reason}",
                        username, response.StatusCode, response.ReasonPhrase);
                    return new KeycloakResponse<KeycloakToken> { Success = false, Message = "Authentication failed." };
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    AllowTrailingCommas = true
                };
                var tokenData = JsonSerializer.Deserialize<KeycloakToken>(responseContent, options);
                var userId = JwtHelper.GetUserIdFromToken(tokenData.AccessToken);
                tokenData.UserId = userId;
                return new KeycloakResponse<KeycloakToken> { Success = true, Data = tokenData };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while authenticating user.");
                return new KeycloakResponse<KeycloakToken> { Success = false, Message = ex.Message };
            }
        }


    }
}
