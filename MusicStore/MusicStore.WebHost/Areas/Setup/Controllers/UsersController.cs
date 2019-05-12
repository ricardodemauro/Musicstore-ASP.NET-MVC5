using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Areas.Setup.Models;
using MusicStore.WebHost.Infrastructure;
using MusicStore.WebHost.Infrastructure.Extensions;
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
    public class UsersController : Controller
    {
        public async Task<IActionResult> Index([FromServices]UserManager<ApplicationUser> userManager, CancellationToken cancellationToken = default)
        {
            var users = await userManager.Users
                .Select(x => new UserViewModel() { Email = x.UserName })
                .ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [ActionName("CreateUser")]
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUserPosted([FromForm]UserViewModel model, [FromServices] RoleManager<IdentityRole> roleManager, [FromServices] UserManager<ApplicationUser> userManager, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return View(model);

            var applicationUser = new ApplicationUser { Email = model.Email, UserName = model.Email };
            var result = await userManager.CreateAsync(applicationUser, model.Password);

            if (!result.Succeeded)
            {
                this.AddIdentityFailedResult(nameof(model.Email), result.Errors);

                return View(model);
            }

            if (model.IsAdmin)
            {
                var adminRole = await roleManager.FindByIdAsync(RoleNames.ADMINISTRATOR);
                if (adminRole == null)
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole { Id = RoleNames.ADMINISTRATOR, Name = RoleNames.ADMINISTRATOR, NormalizedName = RoleNames.ADMINISTRATOR });

                    if (!result.Succeeded)
                    {
                        this.AddIdentityFailedResult(nameof(model.Email), roleResult.Errors);
                        return View(model);
                    }
                }

                var addRoleResult = await userManager.AddToRoleAsync(applicationUser, RoleNames.ADMINISTRATOR);

                if (!addRoleResult.Succeeded)
                {
                    this.AddIdentityFailedResult(nameof(model.Email), addRoleResult.Errors);
                    return View(model);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
