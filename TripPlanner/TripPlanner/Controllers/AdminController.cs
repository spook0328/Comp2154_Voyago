using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TripPlanner.Controllers
{
    // This controller is decorated with the [Authorize] attribute, which means that only authenticated users can access its actions.
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
