using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace hitfit.app.Controllers.App
{
    public class ResultController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}