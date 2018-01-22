using Microsoft.AspNetCore.Mvc;

namespace SpaceRemastered.Controllers
{
	public class AppController : Controller
	{
		public IActionResult Index() => View();

	}
}
