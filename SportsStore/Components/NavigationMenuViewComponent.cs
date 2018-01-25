using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository repository;

        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            //ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(new NavigationMenuViewViewModel
            {
                Categories = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x).ToArray(),
                SelectedCategory = (string)RouteData?.Values["category"]
            });       
        }
    }
}
