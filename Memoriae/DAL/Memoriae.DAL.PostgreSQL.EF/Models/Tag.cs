using System;
using System.Collections.Generic;

#nullable disable

namespace Memoriae.DAL.PostgreSQL.EF.Models
{
    public partial class Tag
    {
        public Tag()
        {
            PostTagLink = new HashSet<PostTagLink>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PostTagLink> PostTagLink { get; set; }
    }
}
