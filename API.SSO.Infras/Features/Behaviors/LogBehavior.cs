using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Features.Behaviors
{
    public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                // TODO handle log
                Console.WriteLine($"{request?.GetType().Name} at {DateTimeOffset.Now:dd-MM-yyyy / H:mm:ss}");

                var res = await next();

                Console.WriteLine($"{request?.GetType().Name} end {DateTimeOffset.Now:dd-MM-yyyy / H:mm:ss}");

                return res;
            }
            catch (Exception)
            {
                // TODO handle exception
                throw;
            }
        }
    }
}
