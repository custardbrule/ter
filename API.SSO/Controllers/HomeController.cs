using Microsoft.AspNetCore.Mvc;

namespace API.SSO.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public string Index()
        {
            return "Server is running";
        }
    }
}
