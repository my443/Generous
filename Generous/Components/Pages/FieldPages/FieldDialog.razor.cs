using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Generous.Models;

namespace Generous.Components.Pages.FieldPages
{
    public partial class FieldDialog
    {
        [Parameter]
        public Field Content { get; set; } = default!;

        [CascadingParameter]
        public FluentDialog? Dialog { get; set; }

        [Inject] public IDbContextFactory<AppDbContext> DbFactory { get; set; } = default!;

        private FieldType? SelectedFieldType
        {
            get => Content.FieldType;
            set
            {
                Content.FieldType = value;
                if (value != null)
                {
                    Content.FieldTypeId = value.Id; // Sync the Foreign Key!
                }

                // Trigger validation logic if needed
                ToggleDialogPrimaryActionButton(!string.IsNullOrWhiteSpace(Content.Name) && value != null);
            }
        }
        private List<FieldType> FieldTypesList { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            await LoadFieldTypesAsync();

            ToggleDialogPrimaryActionButton(!string.IsNullOrWhiteSpace(Content.Name));
        }

        private void ToggleDialogPrimaryActionButton(bool enable)
        {
            Dialog!.TogglePrimaryActionButton(enable);
        }

        private async Task LoadFieldTypesAsync()
        {
            using var _context = DbFactory.CreateDbContext();
            FieldTypesList = await _context.FieldTypes.OrderBy(ft => ft.Id).ToListAsync();

            if (Content?.FieldType != null)
            {
                SelectedFieldType = FieldTypesList?
                            .FirstOrDefault(ft => ft.Id == Content.FieldType.Id);
            }
            else if (SelectedFieldType == null)
            {
                SelectedFieldType = await _context.FieldTypes
                                                 .OrderBy(ft => ft.Id)
                                                 .FirstOrDefaultAsync();
            }
        }
    }
}
