using Printly.Dto.Response;

namespace Printly.Printers
{
    public class CreatePrinterCommandResponse
    {
        public Printer Printer { get; set; }
        public Error Error { get; set; }
    }
}
