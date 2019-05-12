using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Areas.Setup.Models
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
