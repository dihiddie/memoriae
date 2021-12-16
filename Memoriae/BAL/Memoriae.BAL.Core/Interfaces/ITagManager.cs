using Memoriae.BAL.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoriae.BAL.Core.Interfaces
{
    public interface ITagManager
    {
        Task<Tag> CreateOrUpdateAsync(Tag tag);

        Task<bool> CreateAsync(IEnumerable<string> tags);

        Task<IEnumerable<Tag>> GetAsync();
    }
}
