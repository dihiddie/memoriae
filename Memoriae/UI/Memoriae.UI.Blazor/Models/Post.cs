using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Memoriae.UI.Blazor.Models
{
    public class Post
    {
        [Required(ErrorMessage = "* Поле обязательно")]
        public string Title { get; set; }

        [Required(ErrorMessage = "* Поле обязательно")]
        public string Text { get; set; }


        public IEnumerable<Tag> Tags { get; set; }
    }
}
