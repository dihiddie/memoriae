﻿using System;
using System.Collections.Generic;

namespace Memoriae.BAL.Core.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string PreviewText { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<Tag> ExistedTags { get; set; }

        public IEnumerable<Tag> NewTags { get; set; }

        public bool AutoSaved { get; set; }
    }
}
