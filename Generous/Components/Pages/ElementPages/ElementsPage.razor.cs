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

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            StateHasChanged();
        }

        private async Task LoadDataAsync()
        {
            using var _context = DbFactory.CreateDbContext();
            _elementList = await _context.Elements.ToListAsync();

            StateHasChanged();
        }
        private async Task AddElement()
        {
            var newElement = new Element();
            Element? updatedElement = await OpenElementDialog(newElement);

            if (updatedElement != null)
            {

                using var _context = DbFactory.CreateDbContext();
                _context.Add(updatedElement);
                _context.SaveChanges();

                await LoadDataAsync();
            }
        }

        private async Task DeleteItem(Element e) { 
            using var _context = DbFactory.CreateDbContext();
            _context.Remove(e);
            _context.SaveChanges();

            await LoadDataAsync();
        }

        private async Task<Element?> OpenElementDialog(Element element)
        {
            Element returnElement = element;

            DialogParameters parameters = new()
            {
                Title = $"Element Name",
                TitleTypo = Typography.H2,
                PrimaryAction = "Save",
                PrimaryActionEnabled = true,
                SecondaryAction = "Cancel",
                Width = "500px",
                TrapFocus = true,
                Modal = true,
                PreventScroll = true
            };

            IDialogReference dialog = await DialogService.ShowDialogAsync<ElementDialog>(element, parameters);
            DialogResult? result = await dialog.Result;

            if (result.Cancelled)
            {
                return null;
            }          

            return returnElement;
        }

    }
}
