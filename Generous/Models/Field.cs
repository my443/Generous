using Microsoft.VisualBasic.FileIO;

namespace Generous.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string FixedColumnName { get; set; }
        public int FieldTypeId { get; set; }
        public int ElementId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime? ModifiedDate { get; set; }

        public FieldType FieldType { get; set; }

        public Element Element { get; set; }
    }
}
