using Printly.Dto.Response;

namespace Printly.Dto.Request
{
    public class Printer
    {
        public string Id { get; set; }
        public string MarlinId { get; set; }
        public string Name { get; set; }
        public Dimensions BedSize { get; set; }
    }
}
