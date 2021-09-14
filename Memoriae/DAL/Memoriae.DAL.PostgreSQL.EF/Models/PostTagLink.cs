using System;
using System.Collections.Generic;

#nullable disable

namespace Memoriae.DAL.PostgreSQL.EF.Models
{
    public partial class PostTagLink
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
