using Memoriae.BAL.Core.Interfaces;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Post = Memoriae.BAL.Core.Models.Post;
using Tag = Memoriae.BAL.Core.Models.Tag;

namespace Memoriae.BAL.PostgreSQL
{
    public class PostManager : IPostManager
    {
        private readonly PersonalContext context;

        private readonly ILogger<PostManager> logger;

        public PostManager(ILogger<PostManager> logger, PersonalContext context)
        {
            this.context = context;
            this.logger = logger;
        }

        public Task<Post> CreateAsync(Post post)
        {
            throw new NotImplementedException();
        }

        public Task CreateOrUpdatePostTagLinkAsync(Guid postId, IEnumerable<Tag> newTags, IEnumerable<Guid> existingTags)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> GetByTags(IEnumerable<Guid> tagIds)
        {
            throw new NotImplementedException();
        }

        public Task<Post> UpdateAsync(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
