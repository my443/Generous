using Generous.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Generous.Components.Pages.ElementPages
{
    public partial class ElementDialog
    {
        [Parameter]
        public Element Content { get; set; } = default!;

        [CascadingParameter]
        public FluentDialog? Dialog { get; set; }

        private string? NameValue
        {
            get => Content.Name;
            set
            {
                if (Content.Name != value)
                {
                    Content.Name = value;
                    // This fires every time the user types!
                    ToggleDialogPrimaryActionButton(!string.IsNullOrWhiteSpace(value));
                }
            }
        }

        protected override void OnInitialized()
        {
            ToggleDialogPrimaryActionButton(!string.IsNullOrWhiteSpace(Content.Name));
        }

        private void ToggleDialogPrimaryActionButton(bool enable)
        {
            Dialog!.TogglePrimaryActionButton(enable);
        }
    }
}
