using System.Text.Json;

namespace Generous.Models
{
    public class CoreData
    {
        public int Id { get; set; }
        public int ElementId { get; set; }
        public JsonDocument Data { get; set; } // jsonb
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Element Element { get; set; } = new();
    }
}
