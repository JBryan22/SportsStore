using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class NavigationMenuViewViewModel
    {
        public string[] Categories { get; set; }
        public string SelectedCategory { get; set; }
    }
}
