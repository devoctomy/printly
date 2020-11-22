using MongoDB.Bson;

namespace Printly.Domain.Models
{
    public interface IStorageEntity
    {
        public ObjectId Id { get; set; }
    }
}
