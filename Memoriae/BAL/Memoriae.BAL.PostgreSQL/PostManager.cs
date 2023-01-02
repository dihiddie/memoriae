using AutoMapper;
using Memoriae.BAL.Core.Interfaces;
using Memoriae.BAL.Core.Models;
using Memoriae.BAL.PostgreSQL.Extensions;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DbPost = Memoriae.DAL.PostgreSQL.EF.Models.Post;
using DbTag = Memoriae.DAL.PostgreSQL.EF.Models.Tag;
using Post = Memoriae.BAL.Core.Models.Post;
using Tag = Memoriae.BAL.Core.Models.Tag;

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
            mapped.CreateDateTime = DateTime.Now;
            mapped.ChapterNumber = chapterNumber;
            // mapped.Title = $"Глава {chapterNumber}. {mapped.Title}";
            // mapped.Title = mapped.Title;
            await context.AddAsync(mapped).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            var postTags = new PostTags
            {
                PostId = mapped.Id,
                NewTags = post.NewTags?.Select(x => x.Name),
                ExistingTags = post.ExistedTags?.Select(x => x.Id.Value)
            };
            await CreateOrUpdatePostTagLinkAsync(postTags);            

            return await GetAsync(mapped.Id).ConfigureAwait(false);

        }

        public async Task CreateOrUpdatePostTagLinkAsync(PostTags postTags)
        {            
            await SavePostWithExistingTags(postTags.PostId, postTags.ExistingTags).ConfigureAwait(false);
            await SaveNewTagsAndLinksAsync(postTags.PostId, postTags.NewTags).ConfigureAwait(false);            

            logger.LogInformation($"Сохранение поста с Id = {postTags.PostId} с тегами произведено успешно");            
        }

        public async Task<IEnumerable<Post>> GetAsync()
        {
            logger.LogInformation("Получаем список постов кроме черновиков");

            return await context.Posts.AsNoTracking().Where(x => !x.AutoSaved).OrderByDescending(x => x.CreateDateTime).Select(x => new Post()
            {
                Id = x.Id,
                Title = $"Глава {x.ChapterNumber}. {x.Title}",
                PreviewText = x.PreviewText,
                CreateDateTime = x.CreateDateTime,  
                Tags = x.PostTagLink.Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name })

            }).ToListAsync().ConfigureAwait(false);                             
        }

        public async Task<Post> GetAsync(Guid id)
        {
            logger.LogInformation($"Получение поста с id = {id}");            

            return await context.Posts.Select(x => new Post()
            {
                Id = x.Id,
                Text = x.Text,
                Title = x.Title,
                PreviewText = x.PreviewText,
                CreateDateTime = x.CreateDateTime,
                Tags = x.PostTagLink.Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name })

            }).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);           

        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            logger.LogInformation("Получаем список всех постов");

            return await context.Posts.AsNoTracking().OrderByDescending(x => x.CreateDateTime).Select(x => new Post()
            {
                Id = x.Id,                
                Title = $"Глава {x.ChapterNumber}. {x.Title}",
                PreviewText = x.PreviewText,
                CreateDateTime = x.CreateDateTime,
                Tags = x.PostTagLink.Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name }),
                AutoSaved = x.AutoSaved

            }).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Post>> GetByTagsAsync(HashSet<Guid> tagIds)
        {
            logger.LogInformation($"Получение списка постов, у которых идентификаторы тэгов входят в {string.Join(", ", tagIds)}");

            var postsByTagIds = context.PostTagLinks.Where(x => tagIds.Contains(x.TagId)).Select(x => x.PostId).ToHashSet(); 
            return await GetAsync(postsByTagIds).ConfigureAwait(false);
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            logger.LogInformation($"Обновление поста с id = {post.Id}");
            var postInDb = await context.Posts.FirstOrDefaultAsync(x => x.Id == post.Id).ConfigureAwait(false);
            mapper.Map(post, postInDb);
            post.CreateDateTime = postInDb.CreateDateTime;
            context.Update(postInDb);
            await context.SaveChangesAsync(false);

            var postTags = new PostTags
            {
                PostId = post.Id,
                NewTags = post.NewTags?.Select(x => x.Name),
                ExistingTags = post.ExistedTags?.Select(x => x.Id.Value)
            };
            await CreateOrUpdatePostTagLinkAsync(postTags);

            return await GetAsync(post.Id).ConfigureAwait(false);
        }

        private async Task<int> ChapterNumberAsync() => await context.Posts.CountAsync().ConfigureAwait(false) + 1;

        private async Task RemoveExistingPostTagLinks(Guid postId)
        {
            var forRemove = await context.PostTagLinks.Where(x => x.PostId == postId).ToListAsync().ConfigureAwait(false);
            context.PostTagLinks.RemoveRange(forRemove);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task SaveNewTagsAndLinksAsync(Guid postId, IEnumerable<string> newTags)
        {
            if (newTags?.Any() == true)
            {
                var newTagsInDb = new List<DbTag>();

                foreach (var tagName in newTags) newTagsInDb.Add(new DbTag { Name = tagName });                
                await context.AddRangeAsync(newTagsInDb).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                logger.LogInformation($"Сохранение новых тегов с названиями {string.Join(", ", newTags)} успешно");

                await context.AddRangeAsync(newTagsInDb.Select(x => new PostTagLink { PostId = postId, TagId = x.Id }));
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task SavePostWithExistingTags(Guid postId, IEnumerable<Guid> existingTags)
        {
            if (existingTags?.Any() == true)
            {
                await RemoveExistingPostTagLinks(postId).ConfigureAwait(false);

                await context.AddRangeAsync(existingTags.Select(x => new PostTagLink { PostId = postId, TagId = x }));
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task<IEnumerable<Post>> GetAsync(IEnumerable<Guid> ids)
        {            
            return await context.Posts.Select(x => new Post()
            {
                Id = x.Id,
                Text = x.Text,
                Title = $"Глава {x.ChapterNumber}. {x.Title}",
                CreateDateTime = x.CreateDateTime,
                Tags = x.PostTagLink.Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name })

            }).Where(x => ids.Contains(x.Id)).ToListAsync().ConfigureAwait(false);

        }      

        public async Task<IEnumerable<Post>> SearchAsync(string searchText)
        {
            List<Post> foundedPosts = new List<Post>();

            if (string.IsNullOrEmpty(searchText)) return await GetAsync().ConfigureAwait(false);

            var splittedText = searchText.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            await AddFoundedPosts(foundedPosts, splittedText).ConfigureAwait(false);
            await AddFoundedPostsInTags(foundedPosts, splittedText).ConfigureAwait(false);

            return foundedPosts.OrderByDescending(i => i.CreateDateTime);
        }

        private async Task AddFoundedPosts(List<Post> foundedPosts, List<string> splittedText)
        {
            foreach (var searchPattern in splittedText.Select(word => $"(?=.*{word})"))
            {
                var postsQuery = context.Posts.Where(
                    i => Regex.IsMatch(i.Text, searchPattern, RegexOptions.IgnoreCase)
                    || Regex.IsMatch(i.Title, searchPattern, RegexOptions.IgnoreCase));

                var posts = await postsQuery.Select(x => new Post()
                {
                    Id = x.Id,
                    Title = x.Title.WrapWordsInTag(splittedText),
                    PreviewText = x.PreviewText.WrapWordsInTag(splittedText),
                    CreateDateTime = x.CreateDateTime,
                    Tags = x.PostTagLink.Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name.WrapWordsInTag(splittedText) })

                }).AsNoTracking().ToListAsync().ConfigureAwait(false);

                var notAddedPostsIds = posts.Select(x => x.Id).Except(foundedPosts.Select(x => x.Id));
                foundedPosts.AddRange(posts.Where(x => notAddedPostsIds.Contains(x.Id)));
            }
        }

        private async Task AddFoundedPostsInTags(List<Post> foundedPosts, List<string> splittedText)
        {
            foreach (var searchPattern in splittedText.Select(word => $"(?=.*{word})"))
            {
                var tagsQuery = context.Tags.Where(i => Regex.IsMatch(i.Name, searchPattern, RegexOptions.IgnoreCase));
                var postsFromTagsQuery = tagsQuery.SelectMany(x => x.PostTagLink.Select(y => y.Post));
                var postsFromTags = await postsFromTagsQuery.Select(x => new Post()
                {
                    Id = x.Id,
                    Title = x.Title.WrapWordsInTag(splittedText),
                    PreviewText = x.PreviewText.WrapWordsInTag(splittedText),
                    CreateDateTime = x.CreateDateTime,
                    Tags = x.PostTagLink.Select(t => new Tag { Id = t.Tag.Id, Name = t.Tag.Name.WrapWordsInTag(splittedText) })

                }).AsNoTracking().ToListAsync().ConfigureAwait(false);

                var notAddedPostsFromTagsIds = postsFromTags.Select(x => x.Id).Except(foundedPosts.Select(x => x.Id));
                foundedPosts.AddRange(postsFromTags.Where(x => notAddedPostsFromTagsIds.Contains(x.Id)));
            }
        }     
    }
}
