﻿using Memoriae.BAL.Core.Interfaces;
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
        /// <param name="postTags">Модель со связью поста и тегов</param>      
        /// <returns>Нет возвращаемого значения</returns>
        [HttpPost("postTagLink")]
        public Task CreateOrUpdatePostTagLinkAsync(PostTags postTags)
            => postManager.CreateOrUpdatePostTagLinkAsync(postTags);

        /// <summary>
        /// Получение списка постов кроме черновиков
        /// </summary>
        /// <returns>Список постов</returns>
        [HttpGet]
        public Task<IEnumerable<Post>> GetAsync() => postManager.GetAsync();

        /// <summary>
        /// Получение списка всех постов
        /// </summary>
        /// <returns>Список постов</returns>
        [HttpGet("all")]
        public Task<IEnumerable<Post>> GetAllAsync() => postManager.GetAllAsync();

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор поста</param>
        /// <returns>Запрашиваемый пост</returns>
        [HttpGet("{id}")]
        public Task<Post> GetAsync(Guid id) => postManager.GetAsync(id);

        /// <summary>
        /// Получение списка постов по тэгам
        /// </summary>
        /// <param name="tagIds">Идентификаторы тегов</param>
        /// <returns>Список постов по тэгам</returns>
        [HttpPost("tags")]
        public Task<IEnumerable<Post>> GetByTagsAsync(HashSet<Guid> tagIds) => postManager.GetByTagsAsync(tagIds);

        /// <summary>
        /// Обновление поста
        /// </summary>
        /// <param name="post">Модель обновляемого поста</param>
        /// <returns>Обновляемый пост</returns>
        [HttpPut]
        public Task<Post> UpdateAsync(Post post) => postManager.UpdateAsync(post);

        /// <summary>
        /// Получение списка постов по искомой фразе
        /// </summary>
        /// <returns>Список постов</returns>
        [HttpGet("search")]
        public Task<IEnumerable<Post>> GetBySearchAsync(string searchText) => postManager.SearchAsync(searchText);
    }
}
