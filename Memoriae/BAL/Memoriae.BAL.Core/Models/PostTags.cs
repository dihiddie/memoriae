using System;
using System.Collections.Generic;
using System.Text;

namespace Memoriae.BAL.Core.Models
{
    public class PostTags
    {
        public Guid PostId { get; set; }

        public IEnumerable<string> NewTags { get; set; }

        public IEnumerable<Guid> ExistingTags { get; set; }
    }
}
