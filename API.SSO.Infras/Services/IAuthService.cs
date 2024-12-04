using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Services
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateJwt(string email, string password, CancellationToken cancellationToken = default);
        Task<(string AccessToken, string RefreshToken)> RefreshJwt(string token, string refresh, CancellationToken cancellationToken = default);
    }
}
