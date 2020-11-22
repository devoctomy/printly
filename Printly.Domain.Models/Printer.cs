using System;

namespace Printly.Domain.Models
{
    public class Printer : StorageEntityBase
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        public string MarlinId { get; set; }
        public string Name { get; set; }
    }
}
