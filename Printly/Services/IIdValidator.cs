using Printly.Dto.Response;

namespace Printly.Services
{
    public interface IIdValidator
    {
        Error Validate(string id);
    }
}
