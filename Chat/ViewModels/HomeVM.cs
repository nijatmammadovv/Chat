using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<AppUser> Users { get; set; }
        public AppUser CurrentUser { get; set; }
    }
}
