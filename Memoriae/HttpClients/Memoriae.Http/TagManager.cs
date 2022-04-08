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
    public class TagManager : ITagManager
    {
        private readonly HttpClient httpClient;

        public TagManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> CreateAsync(IEnumerable<string> tags)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tags), Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"tags", content).ConfigureAwait(false);
            var data = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<bool>(data);
        }

        public async Task<Tag> CreateOrUpdateAsync(Tag tag)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tag), Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"tag", content).ConfigureAwait(false);
            var data = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Tag>(data);
        }

        public async Task<IEnumerable<Tag>> GetAsync()
        {
            var data = await httpClient.GetStringAsync($"tag").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<Tag>>(data);
        }
    }
}
