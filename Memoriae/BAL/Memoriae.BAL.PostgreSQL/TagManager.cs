using Memoriae.BAL.Core.Interfaces;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tag = Memoriae.BAL.Core.Models.Tag;
using DbTag = Memoriae.DAL.PostgreSQL.EF.Models.Tag;
using AutoMapper;

namespace Memoriae.BAL.PostgreSQL
{
    public class TagManager : ITagManager
    {
        private readonly ILogger<TagManager> logger;

        private readonly PersonalContext context;

        private readonly IMapper mapper;

        public TagManager(ILogger<TagManager> logger, IMapper mapper, PersonalContext context)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> CreateAsync(IEnumerable<string> tags)
        {         
            var tagsInDb = new List<DbTag>();
            foreach (var tagName in tags) tagsInDb.Add(new DbTag { Name = tagName });
            await context.Tags.AddRangeAsync(tagsInDb).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<Tag> CreateOrUpdateAsync(Tag tag)
        {
            logger.LogInformation($"Создаем или обновляем тэг с названием = {tag.Name}");

            DbTag dbTag = null;

            if (tag.Id == null) {
                dbTag = new DbTag { Name = tag.Name };
                await context.AddAsync(dbTag).ConfigureAwait(false);
            }
            else
            {
                dbTag = await context.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id).ConfigureAwait(false);
                dbTag.Name = tag.Name;
            }

            await context.SaveChangesAsync().ConfigureAwait(false);

            return mapper.Map<Tag>(dbTag);
        }       

        public async Task<IEnumerable<Tag>> GetAsync()
        {
            logger.LogInformation("Получаем список тэгов");
            return await context.Tags.AsNoTracking().Select(x => new Tag { Id = x.Id, Name = x.Name }).ToListAsync();
            
        }
    }
}
