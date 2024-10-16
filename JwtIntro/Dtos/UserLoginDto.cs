﻿using System.ComponentModel.DataAnnotations;

namespace JwtIntro.Dtos
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
