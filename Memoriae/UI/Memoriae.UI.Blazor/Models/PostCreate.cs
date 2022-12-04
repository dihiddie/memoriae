using Blazored.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Memoriae.UI.Blazor.Models
{
    public class PostCreate
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "* Поле обязательно")]
        public string Title { get; set; }        

        public IEnumerable<Tag> Tags { get; set; }
    }
}
