using Printly.Dto.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Client
{
    public interface IPrintersClient
    {
        Task<PrintlyResponse<List<Printer>>> GetAllAsync(CancellationToken cancellationToken);
        Task<PrintlyResponse<Printer>> GetAsync(
            string id,
            CancellationToken cancellationToken);
        Task<PrintlyResponse<Printer>> Createsync(
            Dto.Request.Printer printer,
            CancellationToken cancellationToken);
        Task<PrintlyResponse<Printer>> UpdateAsync(
            string id,
            Dto.Request.Printer printer,
            CancellationToken cancellationToken);
        Task<PrintlyResponse<Printer>> DeleteAsync(
            string id,
            CancellationToken cancellationToken);
    }
}
