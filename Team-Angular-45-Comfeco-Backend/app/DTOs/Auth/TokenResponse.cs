﻿using System;

namespace BackendComfeco.DTOs.Auth
{
    public class TokenResponse
    {
        public DateTime Expiration { get; set; }
        public string Token { get; set; }

        public string ResponseType { get; set; } = "Bearer";

        public bool IsPersistent { get; set; } = false;
    }
}
