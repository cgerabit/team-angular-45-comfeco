using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackendComfeco.DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        public bool PersistLogin { get; set; } = false;

        public bool IsEmail => UserName==null  ? false : Regex.IsMatch(UserName, "^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$");

    }
}
