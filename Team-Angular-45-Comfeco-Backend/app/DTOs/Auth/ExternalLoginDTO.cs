using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.DTOs.Auth
{
    public class ExternalLoginDTO
    {
        public string Provider { get; set; }
        public string returnUrl { get; set; }
    }
}
