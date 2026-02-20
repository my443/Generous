using Generous.Components.Pages.ElementPages;
using Generous.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Xml.Linq;

namespace Generous.Components.Pages.FieldPages
{
    public partial class FieldsPage
    {
        [Inject] public IDbContextFactory<AppDbContext> DbFactory { get; set; } = default!;
        [Inject] public IDialogService DialogService { get; set; }
        
        private List<Element> ElementList { get; set; } = new List<Element>();

        private Element? _selectedElement;

        private Element? SelectedElement
        {
            get => _selectedElement;
            set => _selectedElement = value; // Keep this simple
        }
        //private Element SelectedElement { get; set; }

        private List<Field> _fieldList = new List<Field>();
        private IQueryable<Field> FieldList => _fieldList.AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            using var _context = DbFactory.CreateDbContext();
            ElementList = await _context.Elements.ToListAsync();
            await LoadDataAsync();
            await OnElementChanged(SelectedElement);            // To get the list the first time. 
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync() {
            using var _context = DbFactory.CreateDbContext();

            if (SelectedElement == null)
            {
                SelectedElement = ElementList.FirstOrDefault();
            }

            await OnElementChanged(SelectedElement);

            StateHasChanged();
        }

        private async Task OnElementChanged(Element? element) {
            if (element == null) return;

            // Keep the reference from the list we already have
            SelectedElement = ElementList.FirstOrDefault(e => e.Id == element.Id);

            using var _context = DbFactory.CreateDbContext();
            _fieldList = await _context.Fields
                            .Include(f => f.FieldType)
                            .Where(f => f.ElementId == SelectedElement.Id)
                            .OrderBy(f => f.Id)
                            .ToListAsync();
        }

        private async Task AddItem()
        {
            using var _context = DbFactory.CreateDbContext();
            var newField = new Field();

            //newField.Element = await _context.Elements.Where(e => e.Id == SelectedElement.Id).FirstOrDefaultAsync();

            Field? updatedField = await OpenFieldDialog(newField);

            if (updatedField != null)
            {
                updatedField.FixedColumnName = updatedField.Name.Replace(" ", "_").ToLower();
                updatedField.ElementId = SelectedElement.Id;

                // This has to be set to null, or else it tries to updated it from FieldTypeId
                // and from this feild type. And it fails this because of a db constraint.                
                updatedField.FieldType = null;

                _context.Add(updatedField);
                _context.SaveChanges();

                await LoadDataAsync();
            }
            StateHasChanged();
        }

        private async Task<Field?> OpenFieldDialog(Field field)
        {
            Field returnField = field;
            Field savedField = CreateFieldCopy(field);

            DialogParameters parameters = new()
            {
                Title = $"Feild For: {SelectedElement.Name}",
                TitleTypo = Typography.H2,
                PrimaryAction = "Save",
                PrimaryActionEnabled = true,
                SecondaryAction = "Cancel",
                Width = "500px",
                TrapFocus = true,
                Modal = true,
                PreventScroll = true
            };

            IDialogReference dialog = await DialogService.ShowDialogAsync<FieldDialog>(field, parameters);
            DialogResult? result = await dialog.Result;

            if (result.Cancelled)
            {
                return savedField;
            }

            return returnField;
        }

        private async Task DeleteItem(Field field)
        {
            // 1. Show the built-in confirmation dialog
            var dialog = await DialogService.ShowConfirmationAsync(
                message: $"Are you sure you want to delete '{field.Name}'?",
                primaryText: "Delete",
                secondaryText: "Cancel",
                title: "Confirm Delete");

            var result = await dialog.Result;

            // 2. Check if the user clicked "Delete" (Primary Action)
            // result.Cancelled is true if they click "Cancel" or the 'X'
            if (!result.Cancelled)
            {
                using var _context = DbFactory.CreateDbContext();
                _context.Remove(field);
                _context.SaveChanges();

                await LoadDataAsync();
            }
        }

        private async Task EditItem(Field field)
        {
            Field? updatedField = await OpenFieldDialog(field);

            if (updatedField != null)
            {
                updatedField.ModifiedDate = DateTime.Now.ToUniversalTime();

                using var _context = DbFactory.CreateDbContext();
                _context.Update(updatedField);
                _context.SaveChanges();

                await LoadDataAsync();
            }
        }

        private Field CreateFieldCopy(Field field)
        {
            return new Field
            {
                Id = field.Id,
                Name = field.Name,
                Description = field.Description,
                CreateDate = field.CreateDate,
                ModifiedDate = field.ModifiedDate,
                Element = field.Element,
                FieldType = field.FieldType,
            };
        }
    }
}
