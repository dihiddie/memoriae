using Memoriae.BAL.Core.Interfaces;
using Memoriae.BAL.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoriae.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostManager postManager;

        public PostController(IPostManager postManager) => this.postManager = postManager;

        /// <summary>
        /// Создание поста
        /// </summary>
        /// <param name="post">Создаваемый пост</param>
        /// <returns>Созданный пост</returns>
        [HttpPost]
        public Task<Post> CreateAsync(Post post) => postManager.CreateAsync(post);

        /// <summary>
        /// Создание или обновление связей между постом и тегами
        /// </summary>
        /// <param name="postId">Идентификатор поста</param>
        /// <param name="newTags">Новые теги</param>
        /// <param name="existingTags">Существующие теги</param>
        /// <returns></returns>
        //[HttpPost]
        //public Task CreateOrUpdatePostTagLinkAsync(Guid postId, IEnumerable<Tag> newTags, IEnumerable<Guid> existingTags)
        //    => postManager.CreateOrUpdatePostTagLinkAsync(postId, newTags, existingTags);

        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <returns>Список постов</returns>
        [HttpGet]
        public Task<IEnumerable<Post>> Get() => postManager.Get();

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор поста</param>
        /// <returns>Запрашиваемый пост</returns>
        [HttpGet("id")]
        public Task<Post> Get(Guid id) => postManager.Get(id);

        /// <summary>
        /// Получение списка постов по тэгам
        /// </summary>
        /// <param name="tagIds">Идентификаторы тегов</param>
        /// <returns>Список постов по тэгам</returns>
        [HttpGet("tags")]
        public Task<IEnumerable<Post>> GetByTags(IEnumerable<Guid> tagIds) => postManager.GetByTags(tagIds);

        /// <summary>
        /// Обновление поста
        /// </summary>
        /// <param name="post">Модель обновляемого поста</param>
        /// <returns>Обновляемый пост</returns>
        [HttpPut]
        public Task<Post> UpdateAsync(Post post) => postManager.UpdateAsync(post);
    }
}
