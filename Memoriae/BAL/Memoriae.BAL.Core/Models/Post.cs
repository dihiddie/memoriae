using System;
using System.Collections.Generic;
using System.Text;

namespace Memoriae.BAL.Core.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
