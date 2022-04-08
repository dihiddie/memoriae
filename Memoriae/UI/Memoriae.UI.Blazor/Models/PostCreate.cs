using Blazored.TextEditor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Memoriae.UI.Blazor.Models
{
    public class PostCreate
    {
        [Required(ErrorMessage = "* Поле обязательно")]
        public string Title { get; set; }

        [Required(ErrorMessage = "* Поле обязательно")]
        public BlazoredTextEditor Text { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
