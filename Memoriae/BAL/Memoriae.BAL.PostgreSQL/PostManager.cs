using AutoMapper;
using Memoriae.BAL.Core.Interfaces;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Post = Memoriae.BAL.Core.Models.Post;
using Tag = Memoriae.BAL.Core.Models.Tag;
using DbPost = Memoriae.DAL.PostgreSQL.EF.Models.Post;
using DbTag = Memoriae.DAL.PostgreSQL.EF.Models.Tag;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Memoriae.BAL.PostgreSQL
{
    public class PostManager : IPostManager
    {
        private readonly PersonalContext context;

        private readonly ILogger<PostManager> logger;

        private readonly IMapper mapper;

        public PostManager(ILogger<PostManager> logger, IMapper mapper, PersonalContext context)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            var chapterNumber = ChapterNumber();

            logger.LogInformation($"Попытка создания поста с главой = {chapterNumber}");

            var mapped = mapper.Map<DbPost>(post);
            mapped.Title = $"Глава {chapterNumber}. {mapped.Title}";

            await context.AddAsync(mapped).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return post;

        }

        public Task CreateOrUpdatePostTagLinkAsync(Guid postId, IEnumerable<Tag> newTags, IEnumerable<Guid> existingTags)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Post>> Get()
        {
            logger.LogInformation("Получаем список постов");
            return await context.Posts.AsNoTracking().Select(x => new Post()
            {
                Id = x.Id,
                Text = x.Text,
                Title = x.Title,
                CreateDateTime = x.CreateDateTime,
                Tags = context.PostTagLinks.Where(y => y.PostId == x.Id).Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name})
                
            }).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Post> Get(Guid id)
        {
            logger.LogInformation($"Получение поста с id = {id}");
            return mapper.Map<Post>(await context.Posts.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false));

        }

        public async Task<IEnumerable<Post>> GetByTags(IEnumerable<Guid> tagIds)
        {
            logger.LogInformation($"Получение списка постов, у которых идентификаторы тэгов входят в {string.Join(", ", tagIds)}");
            var posts = await context.PostTagLinks.Where(x => tagIds.Contains(x.TagId)).Select(p => new Post
            {
                Id = p.Post.Id,
                Text = p.Post.Text,
                Title = p.Post.Title,
                CreateDateTime = p.Post.CreateDateTime,
                Tags = context.PostTagLinks.Where(y => y.PostId == p.PostId).Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name })
            }).ToListAsync().ConfigureAwait(false);
            return posts;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            logger.LogInformation($"Обновление поста с id = {post.Id}");
            var postInDb = await context.Posts.FirstOrDefaultAsync(x => x.Id == post.Id).ConfigureAwait(false);
            mapper.Map(post, postInDb);
            context.Update(postInDb);
            await context.SaveChangesAsync(false);

            return post;
        }

        private Task<int> ChapterNumber() => context.Posts.CountAsync();
    }
}
