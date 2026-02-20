using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Generous.Models;

namespace Generous.Components.Pages.DataPages
{
    public partial class Data
    {
        [Parameter] public int DataElementId { get; set; }
        [Inject] public IDbContextFactory<AppDbContext> DbFactory { get; set; } = default!;
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private Element Element { get; set; } 

        private List<Element> Elements { get; set; } = new List<Element>();

        protected override async Task OnInitializedAsync() {
            using var _context = DbFactory.CreateDbContext();
            Element = await _context.Elements.Where(e => e.Id == DataElementId).FirstOrDefaultAsync();
            Elements = await _context.Elements.ToListAsync();
        }

        private async Task ElementClick(FluentDataGridRow<Element> args) {
            NavigationManager.NavigateTo($"/data/{args.Item.Id}");
        }
    }
}
