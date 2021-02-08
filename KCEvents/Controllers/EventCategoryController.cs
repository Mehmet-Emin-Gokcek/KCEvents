using KCEvents.Data;
using KCEvents.Models;
using KCEvents.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCEvents.Controllers
{
    [Authorize]
    public class EventCategoryController: Controller
    { 

        private EventDbContext context;

        public EventCategoryController(EventDbContext dbContext)
        {
            context = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //Querry
            List<EventCategory> categories = context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create() {

            return View(new AddEventCategoryViewModel());
        }

        [HttpPost("/EventCategory/Add")]
        public IActionResult ProcessCreateEventCategoryForm(AddEventCategoryViewModel addEventCategoryViewModel) 
        {
            if (ModelState.IsValid)
            {
                EventCategory newEventCategory = new EventCategory(addEventCategoryViewModel.Name);

                context.Categories.Add(newEventCategory);
                context.SaveChanges();

                return Redirect("/EventCategory");
            }

            else
            {
                return View("Create", addEventCategoryViewModel);
            }
        }

    }
}
