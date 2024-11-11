using System.Linq.Expressions;

namespace API.SSO.Models
{
    public record ErrorResponseModel(string Message, object? Errors = null);
}
