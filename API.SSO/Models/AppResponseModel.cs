namespace API.SSO.Models
{
    public record AppResponseModel<T>(T Data, object? AdditionalData = null);
}
