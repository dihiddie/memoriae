using System;
using System.Collections.Generic;

#nullable disable

namespace Memoriae.DAL.PostgreSQL.EF.Models
{
    public partial class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
