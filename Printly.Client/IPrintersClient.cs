using Printly.Dto.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Client
{
    public interface IPrintersClient
    {
        Task<ObjectResponse<List<Printer>>> GetAllAsync(CancellationToken cancellationToken);
        Task<ObjectResponse<Printer>> GetAsync(
            string id,
            CancellationToken cancellationToken);
        Task<ObjectResponse<Printer>> CreateAsync(
            Dto.Request.Printer printer,
            CancellationToken cancellationToken);
        Task<Response> UpdateAsync(
            string id,
            Dto.Request.Printer printer,
            CancellationToken cancellationToken);
        Task<Response> DeleteAsync(
            string id,
            CancellationToken cancellationToken);
    }
}
