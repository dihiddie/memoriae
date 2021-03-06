using System;
using System.Collections.Generic;

namespace Memoriae.UI.Blazor.Models
{
    public class PostRead
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreateDateTime { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
