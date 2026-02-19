using Generous.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Generous.Components.Pages.FieldPages
{
    public partial class FieldsPage
    {
        [Inject] public IDbContextFactory<AppDbContext> DbFactory { get; set; } = default!;
        [Inject] public IDialogService DialogService { get; set; }
        
        private List<Element> ElementList { get; set; } = new List<Element>();
        private Element SelectedElement { get; set; }

        private List<Field> _fieldList = new List<Field>();
        private IQueryable<Field> Fields => _fieldList.AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync() {
            using var _context = DbFactory.CreateDbContext();
            //_fieldList = await _context.Fields
            //    .Where(f => f.Elements.Select(e => e.Id).Contains(SelectedElement.Id))
            //    .ToListAsync();

            ElementList = await _context.Elements.ToListAsync();
            SelectedElement = await _context.Elements.FirstOrDefaultAsync();
            StateHasChanged();
        }
    }
}
