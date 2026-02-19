using Generous.Models;
using Generous.Components.Pages.ElementPages;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;


namespace Generous.Components.Pages.ElementPages
{
    public partial class ElementsPage
    {
        [Inject] public IDbContextFactory<AppDbContext> DbFactory { get; set; } = default!;
        [Inject] public IDialogService DialogService { get; set; }

        private List<Element> _elementList = new List<Element>();
        private IQueryable<Element> Elements => _elementList.AsQueryable();

        protected override async Task OnInitializedAsync() {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync() {
            using var _context = DbFactory.CreateDbContext();
            _elementList = await _context.Elements.ToListAsync();
        }
        private async Task AddElement() {
            var newElement = new Element();

            DialogParameters parameters = new()
            {
                Title = $"Element Name",
                TitleTypo = Typography.H2,
                PrimaryAction = "Yes",
                PrimaryActionEnabled = true,
                SecondaryAction = "No",
                Width = "500px",
                TrapFocus = true,
                Modal = true,
                PreventScroll = true
            };

            IDialogReference dialog = await DialogService.ShowDialogAsync<ElementDialog>(newElement, parameters);
            DialogResult? result = await dialog.Result;
            
            using var _context = DbFactory.CreateDbContext();
            _context.Add(result);
            _context.SaveChanges();
            StateHasChanged();
        }
    }
}
