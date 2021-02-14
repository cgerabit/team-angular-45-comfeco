using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamAngular45Backend.DTOs.Auth
{
    public class ExternalLoginDTO
    {
        public string Provider { get; set; }
        public string returnUrl { get; set; }
    }
}
