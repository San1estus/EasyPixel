using System;
using System.Collections.Generic;
using System.Text;

namespace CrochetIt.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}