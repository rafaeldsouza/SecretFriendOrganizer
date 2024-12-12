using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretFriendOrganizer.Application.DTOs
{
    public class KeycloakResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class KeycloakResponse<T> : KeycloakResponse
    {
        public T? Data { get; set; }
    }
}
