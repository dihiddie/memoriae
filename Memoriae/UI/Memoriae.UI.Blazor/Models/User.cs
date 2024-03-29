﻿namespace Memoriae.UI.Blazor.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Required(ErrorMessage = "* Поле обязательно")]
        public string Login { get; set; }


        [Required(ErrorMessage = "* Поле обязательно")]
        [MinLength(5, ErrorMessage = "* Пароль должен содержать минимум 5 символов")]
        public string Password { get; set; }
    }
}
