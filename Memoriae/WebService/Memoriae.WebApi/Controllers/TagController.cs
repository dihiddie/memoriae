using Memoriae.BAL.Core.Interfaces;
using Memoriae.BAL.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoriae.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagManager tagManager;

        public TagController(ITagManager tagManager) => this.tagManager = tagManager;

        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <returns>Список тегов</returns>
        [HttpGet]        
        public Task<IEnumerable<Tag>> GetAsync() => tagManager.GetAsync();

        /// <summary>
        /// Создание или обновление тега
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>Созданный или обновленный тег</returns>
        [HttpPost]
        public Task<Tag> CreateOrUpdateAsync(Tag tag) => tagManager.CreateOrUpdateAsync(tag);
    }
}
