using MongoDB.Bson;

namespace Printly.Domain.Models
{
    public class Printer : StorageEntityBase
    {
        public override ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string MarlinId { get; set; }
        public string Name { get; set; }
        public int BedSizeX { get; set; }
        public int BedSizeY { get; set; }
        public int BedSizeZ { get; set; }
    }
}
