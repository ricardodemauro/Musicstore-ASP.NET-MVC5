using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Infrastructure.Extensions
{
    public static class IdentityControllerExtensions
    {
        public static void AddIdentityFailedResult(this Controller controller, string fieldName, IEnumerable<IdentityError> errors)
        {
            foreach (var item in errors)
            {
                controller.ModelState.TryAddModelError(fieldName, item.Description);
            }
        }
    }
}
