using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamAngular45Backend.DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        public bool PersistLogin { get; set; } = false;

        public bool IsEmail => Regex.IsMatch(UserName, "^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$");

    }
}
