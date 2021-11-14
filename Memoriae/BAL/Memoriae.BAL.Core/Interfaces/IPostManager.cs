using Memoriae.BAL.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoriae.BAL.Core.Interfaces
{
    public interface IPostManager
    {
        Task<Post> CreateAsync(Post post);

        Task<Post> UpdateAsync(Post post);

        Task CreateOrUpdatePostTagLinkAsync(Guid postId, IEnumerable<Tag> newTags, IEnumerable<Guid> existingTags);

        Task<IEnumerable<Post>> GetByTags(IEnumerable<Guid> tagIds);

        Task<IEnumerable<Post>> Get();

        Task<Post> Get(Guid id);        
    }
}