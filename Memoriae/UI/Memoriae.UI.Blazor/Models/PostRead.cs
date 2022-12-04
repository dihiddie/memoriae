using System;
using System.Collections.Generic;

namespace Memoriae.UI.Blazor.Models
{
    public class PostRead
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string PreviewText { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
