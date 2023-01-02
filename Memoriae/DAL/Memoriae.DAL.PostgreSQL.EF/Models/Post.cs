using System;
using System.Collections.Generic;

#nullable disable

namespace Memoriae.DAL.PostgreSQL.EF.Models
{
    public partial class Post
    {
        public Post()
        {
            PostTagLink = new HashSet<PostTagLink>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string PreviewText { get; set; }

        public int ChapterNumber { get; set; }

        public DateTime CreateDateTime { get; set; }

        public virtual ICollection<PostTagLink> PostTagLink { get; set; }

        public bool AutoSaved { get; set; }
    }
}
