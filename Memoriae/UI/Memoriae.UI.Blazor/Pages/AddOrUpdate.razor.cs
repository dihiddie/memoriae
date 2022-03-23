using Memoriae.UI.Blazor.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Memoriae.UI.Blazor.Pages
{

    public partial class AddOrUpdate
    {
        private Post post = new Post();

        private EditContext editContext;

        protected override void OnInitialized()
        {
            editContext = new EditContext(post);
        }
    }
}
