using System;
using System.Collections.Generic;

namespace Memoriae.WebApi.Models
{
    public class PostTagsView
    {
       public Guid PostId { get; set; }

        public IEnumerable<string> NewTags { get; set; }
        
        public IEnumerable<Guid> ExistingTags { get; set; }
    }
}
