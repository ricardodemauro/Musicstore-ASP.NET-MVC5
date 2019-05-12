using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Areas.Setup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MusicStore.WebHost.Infrastructure.Extensions;

namespace MusicStore.WebHost.Areas.Setup.Controllers
{
    [Area("Setup")]
    public class RolesController : Controller
    {
        public async Task<IActionResult> Index([FromServices] RoleManager<IdentityRole> roleManager, CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<RoleViewModel> roles = await roleManager.Roles
                .Select(x => new RoleViewModel { Name = x.Id })
                .ToListAsync();

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] RoleViewModel model, [FromServices] RoleManager<IdentityRole> roleManager, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingRole = await roleManager.FindByNameAsync(model.Name);
            if (existingRole != null)
            {
                ModelState.AddModelError(nameof(model.Name), $"Role {model.Name} already exist");
                return View(model);
            }

            var result = await roleManager.CreateAsync(new IdentityRole { Id = model.Name, Name = model.Name, NormalizedName = model.Name });
            if (!result.Succeeded)
            {
                this.AddIdentityFailedResult(nameof(model.Name), result.Errors);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
