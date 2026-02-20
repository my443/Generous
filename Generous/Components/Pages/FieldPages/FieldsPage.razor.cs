using Generous.Components.Pages.ElementPages;
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
            using var _context = DbFactory.CreateDbContext();            
            SelectedElement = await _context.Elements.FirstOrDefaultAsync();

            await LoadDataAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync() {
            using var _context = DbFactory.CreateDbContext();

            ElementList = await _context.Elements.ToListAsync();
            

            //_fieldList = await _context.Fields
            //        .Where(f => f.Elements.Select(e => e.Id).Contains(SelectedElement.Id))
            //        .ToListAsync();

            StateHasChanged();
        }

        private async Task AddItem()
        {
            var newField = new Field();
            Field? updatedField = await OpenFieldDialog(newField);

            if (updatedField != null)
            {
                using var _context = DbFactory.CreateDbContext();

                updatedField.FixedColumnName = updatedField.Name.Replace(" ", "_").ToLower();
                updatedField.ElementId = SelectedElement.Id;
                
                // This has to be set to null, or else it tries to updated it from FieldTypeId
                // and from this feild type. And it fails this because of a db constraint.                
                updatedField.FieldType = null;

                _context.Add(updatedField);
                _context.SaveChanges();

                await LoadDataAsync();
            }
        }

        private async Task<Field?> OpenFieldDialog(Field field)
        {
            Field returnField = field;
            Field savedField = CreateFieldCopy(field);

            DialogParameters parameters = new()
            {
                Title = $"Feild Name",
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
