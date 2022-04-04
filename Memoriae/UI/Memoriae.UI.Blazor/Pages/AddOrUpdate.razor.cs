using Blazored.TextEditor;
using Memoriae.BAL.Core.Interfaces;
using Memoriae.UI.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Memoriae.UI.Blazor.Pages
{

    public partial class AddOrUpdate
    {
        [Inject]
        public IPostManager PostManager { get; set; }

        BlazoredTextEditor QuillHtml;

        private Post post = new Post();        

        protected override void OnInitialized()
        {            
        }

        public async void GetHTML()
        {
            var content = await this.QuillHtml.GetHTML();
            StateHasChanged();
        }

        protected async Task SaveAsync()
        {
            post.Text = await this.QuillHtml.GetHTML().ConfigureAwait(false);

            await PostManager.CreateAsync(new BAL.Core.Models.Post { Text = post.Text, Title = post.Title }).ConfigureAwait(false);
        }
    }
}
