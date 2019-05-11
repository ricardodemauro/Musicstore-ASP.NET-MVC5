using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Areas.Setup.Models;
using MusicStore.WebHost.Infrastructure;
using MusicStore.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Areas.Setup.Controllers
{
    [Area("Setup")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Roles([FromServices] RoleManager<IdentityRole> roleManager, CancellationToken cancellationToken = default)
        {
            List<IdentityRole> roles = await roleManager.Roles.ToListAsync(cancellationToken);
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [ActionName("CreateUser")]
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUserPosted([FromForm]UserViewModel data, [FromServices] RoleManager<IdentityRole> roleManager, [FromServices] UserManager<ApplicationUser> userManager, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return View(data);


            var applicationUser = new ApplicationUser { Email = data.Email, UserName = data.Email };
            var result = await userManager.CreateAsync(applicationUser, data.Password);

            if (!result.Succeeded)
            {
                AddIdentityFailedResult(nameof(data.Email), result.Errors);

                return View(data);
            }

            var adminRole = await roleManager.FindByIdAsync(RoleNames.ADMINISTRATOR);
            if (adminRole == null)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole { Id = RoleNames.ADMINISTRATOR, Name = RoleNames.ADMINISTRATOR, NormalizedName = RoleNames.ADMINISTRATOR });

                if (!result.Succeeded)
                {
                    AddIdentityFailedResult(nameof(data.Email), roleResult.Errors);
                    return View(data);
                }
                adminRole = await roleManager.FindByIdAsync(RoleNames.ADMINISTRATOR);
            }

            var addRoleResult = await userManager.AddToRoleAsync(applicationUser, RoleNames.ADMINISTRATOR);

            if (!addRoleResult.Succeeded)
            {
                AddIdentityFailedResult(nameof(data.Email), addRoleResult.Errors);
                return View(data);
            }
            return RedirectToAction(nameof(Index));
        }

        private void AddIdentityFailedResult(string fieldName, IEnumerable<IdentityError> errors)
        {
            foreach (var item in errors)
            {
                ModelState.TryAddModelError(fieldName, item.Description);
            }
        }
    }
}
