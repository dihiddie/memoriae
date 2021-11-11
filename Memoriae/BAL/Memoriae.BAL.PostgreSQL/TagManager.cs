using Memoriae.BAL.Core.Interfaces;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tag = Memoriae.BAL.Core.Models.Tag;

namespace Memoriae.BAL.PostgreSQL
{
    public class TagManager : ITagManager
    {
        private readonly ILogger<TagManager> logger;

        private readonly PersonalContext context;

        public TagManager(ILogger<TagManager> logger, PersonalContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public Task<Tag> CreateOrUpdateAsync(Tag tag)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Tag>> GetAsync()
        {
            logger.LogInformation("Получаем список тэгов");
            
        }
    }
}
