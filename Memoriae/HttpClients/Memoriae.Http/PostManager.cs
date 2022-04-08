using Memoriae.BAL.Core.Interfaces;
using Memoriae.BAL.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Memoriae.Http.Managers
{
    public class PostManager : IPostManager
    {
        private readonly HttpClient httpClient;

        public PostManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }       

        public async Task<Post> CreateAsync(Post post)
        {
            var content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"post", content).ConfigureAwait(false);
            var data = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Post>(data);
        }

        public async Task CreateOrUpdatePostTagLinkAsync(PostTags postTags)
        {
            var content = new StringContent(JsonConvert.SerializeObject(postTags), Encoding.UTF8, "application/json");
            await httpClient.PostAsync($"posttaglink", content).ConfigureAwait(false);                       
        }

        public async Task<IEnumerable<Post>> GetAsync()
        {
            var data = await httpClient.GetStringAsync($"post").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(data);
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var data = await httpClient.GetStringAsync($"post/{id}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Post>(data);
        }

        public async Task<IEnumerable<Post>> GetByTagsAsync(HashSet<Guid> tagIds)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tagIds), Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"post/tags", content).ConfigureAwait(false);
            var data = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(data);
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            var content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PutAsync($"post", content).ConfigureAwait(false);
            var data = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Post>(data);
        }
    }
}
