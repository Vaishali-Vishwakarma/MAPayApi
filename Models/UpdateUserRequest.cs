﻿using System.ComponentModel.DataAnnotations;

namespace MAPay.Models
{
    public class UpdateUserRequest
    {
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
