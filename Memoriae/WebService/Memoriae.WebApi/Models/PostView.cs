using Memoriae.BAL.Core.Models;
using System.Collections.Generic;

namespace Memoriae.WebApi.Models
{
    public class PostView
    {
        public string Title { get; set; }

        public string Text { get; set; }


        public IEnumerable<Tag> Tags { get; set; }
    }
}
