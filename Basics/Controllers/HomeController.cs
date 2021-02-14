using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
  public class HomeController : Controller
  {

    public IActionResult Index()
    {
      return View();
    }

    [Authorize]
    public IActionResult Secret()
    {
      return View();
    }


  }
}
