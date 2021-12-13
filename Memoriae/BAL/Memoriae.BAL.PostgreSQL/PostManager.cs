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
            var chapterNumber = await ChapterNumberAsync().ConfigureAwait(false);

            logger.LogInformation($"Попытка создания поста с главой = {chapterNumber}");

            var mapped = mapper.Map<DbPost>(post);
            mapped.Title = $"Глава {chapterNumber}. {mapped.Title}";

            await context.AddAsync(mapped).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return mapper.Map<Post>(mapped);

        }

        public async Task CreateOrUpdatePostTagLinkAsync(Guid postId, IEnumerable<string> newTags, IEnumerable<Guid> existingTags)
        {
            var forRemove = await context.PostTagLinks.Where(x => x.Id == postId).ToListAsync().ConfigureAwait(false);
            context.PostTagLinks.RemoveRange(forRemove);

            if (newTags.Any())
            {
                var newTagsInDb = newTags.Select(x => new DbTag { Name = x });
                await context.AddRangeAsync(newTagsInDb).ConfigureAwait(false);
                logger.LogInformation($"Сохранение новых тегов с названиями {string.Join(", ", newTags)} успешно");
                await context.AddRangeAsync(newTagsInDb.Select(x => new PostTagLink { PostId = postId, TagId = x.Id }));
            }

            if (existingTags.Any())
            {                
                await context.AddRangeAsync(existingTags.Select(x => new PostTagLink { PostId = postId, TagId = x }));
            }

            await context.SaveChangesAsync().ConfigureAwait(false);

            logger.LogInformation($"Сохранение поста с Id = {postId} с тегами произведено успешно");
            
        }

        public async Task<IEnumerable<Post>> Get()
        {
            logger.LogInformation("Получаем список постов");

            var posts = await context.Posts.AsNoTracking().Select(x => new Post()
            {
                Id = x.Id,
                Text = x.Text,
                Title = x.Title,
                CreateDateTime = x.CreateDateTime,                

            }).ToListAsync().ConfigureAwait(false);

            foreach(var post in posts)
                post.Tags = await context.PostTagLinks.Where(x => x.PostId == post.Id).Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name }).ToListAsync().ConfigureAwait(false);

            return posts;         
        }

        public async Task<Post> Get(Guid id)
        {
            logger.LogInformation($"Получение поста с id = {id}");

            var tagposts = await context.PostTagLinks.Where(x => x.PostId == id).Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name }).ToListAsync().ConfigureAwait(false);

            return await context.Posts.Select(x => new Post()
            {
                Id = x.Id,
                Text = x.Text,
                Title = x.Title,
                CreateDateTime = x.CreateDateTime,
                Tags = tagposts

            }).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);           

        }

        public async Task<IEnumerable<Post>> GetByTags(HashSet<Guid> tagIds)
        {
            logger.LogInformation($"Получение списка постов, у которых идентификаторы тэгов входят в {string.Join(", ", tagIds)}");
            var posts = await context.PostTagLinks.Where(x => tagIds.Contains(x.TagId)).Select(p => new Post
            {
                Id = p.Post.Id,
                Text = p.Post.Text,
                Title = p.Post.Title,
                CreateDateTime = p.Post.CreateDateTime
            }).ToListAsync().ConfigureAwait(false);

            foreach (var post in posts)
                post.Tags = await context.PostTagLinks.Where(x => x.PostId == post.Id).Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name }).ToListAsync().ConfigureAwait(false);
            return posts;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            logger.LogInformation($"Обновление поста с id = {post.Id}");
            var postInDb = await context.Posts.FirstOrDefaultAsync(x => x.Id == post.Id).ConfigureAwait(false);
            mapper.Map(post, postInDb);
            context.Update(postInDb);
            await context.SaveChangesAsync(false);

            var newTags = post.Tags.Where(x => x.Id == null).Select(x => x.Name);
            await CreateOrUpdatePostTagLinkAsync(post.Id, newTags, null);

            return post;
        }

        private async Task<int> ChapterNumberAsync() => await context.Posts.CountAsync().ConfigureAwait(false) + 1;
    }
}
