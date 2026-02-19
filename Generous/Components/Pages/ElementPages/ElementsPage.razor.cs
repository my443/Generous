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

        private async Task DeleteItem(Element element)
        {

            // 1. Show the built-in confirmation dialog
            var dialog = await DialogService.ShowConfirmationAsync(
                message: $"Are you sure you want to delete '{element.Name}'?",
                primaryText: "Delete",
                secondaryText: "Cancel",
                title: "Confirm Delete");

            var result = await dialog.Result;

            // 2. Check if the user clicked "Delete" (Primary Action)
            // result.Cancelled is true if they click "Cancel" or the 'X'
            if (!result.Cancelled)
            {
                using var _context = DbFactory.CreateDbContext();
                _context.Remove(element);
                _context.SaveChanges();

                await LoadDataAsync();
            }
        }

        private async Task EditItem(Element element)
        {
            Element? updatedElement = await OpenElementDialog(element);

            if (updatedElement != null)
            {
                updatedElement.ModifiedDate = DateTime.Now.ToUniversalTime();

                using var _context = DbFactory.CreateDbContext();
                _context.Update(updatedElement);
                _context.SaveChanges();

                await LoadDataAsync();
            }
        }

        private async Task HandleRowClick(FluentDataGridRow<Element> args)
        {
            var clickedElement = args.Item;
            await EditItem(clickedElement);
        }

        private async Task<Element?> OpenElementDialog(Element element)
        {
            Element returnElement = element;
            Element savedElement = CreateElementCopy(element);

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
                return savedElement;
            }

            return returnElement;
        }

        // Returns a copy of the element to save later. 
        private Element CreateElementCopy(Element element)
        {
            return new Element
            {
                Id = element.Id,
                Name = element.Name,
                Description = element.Description,
                CreatedDate = element.CreatedDate,
                ModifiedDate = element.ModifiedDate,
                Fields = element.Fields
            };
        }

    }
}
