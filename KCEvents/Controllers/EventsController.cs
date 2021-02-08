using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KCEvents.Data;
using KCEvents.Models;
using KCEvents.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace KCEvents.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {

        private readonly EventDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EventsController(EventDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            context = dbContext;
            webHostEnvironment = hostEnvironment;

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //List<Event> events = new List<Event>(EventData.GetAll());
            List<Event> events = context.Events.Include(e => e.Category).Include(e => e.Address).ToList();

            return View(events);
        }

        public IActionResult Add()
        {
            List<EventCategory> categories = context.Categories.ToList();

            EventAddress eventAddress = new EventAddress();

            AddEventViewModel addEventViewModel = new AddEventViewModel(categories, eventAddress);

            return View(addEventViewModel);
        }



        [HttpPost]
        public IActionResult Add(AddEventViewModel addEventViewModel)
        {
            Console.WriteLine("About to add an event....");


            if (ModelState.IsValid)
            {
                //Event newEvent = new Event(addEventViewModel.Name, addEventViewModel.Description, addEventViewModel.ContactEmail);
                //EventData.Add(newEvent);
                EventCategory theCategory = context.Categories.Find(addEventViewModel.CategoryId);

                EventAddress address = new EventAddress
                {
                    Street = addEventViewModel.eventAddress.Street,
                    City = addEventViewModel.eventAddress.City,
                    State = addEventViewModel.eventAddress.State,
                    Zipcode = addEventViewModel.eventAddress.Zipcode,
                    Latitude = addEventViewModel.eventAddress.Latitude,
                    Longitude = addEventViewModel.eventAddress.Longitude,
                };

                string uniqueFileName = UploadedFile(addEventViewModel);

                Event newEvent = new Event
                {
                    Name = addEventViewModel.Name,
                    Description = addEventViewModel.Description,
                    ContactEmail = addEventViewModel.ContactEmail,
                    Category = theCategory,
                    Address = address,
                    Picture = uniqueFileName,
                };


                context.Addresses.Add(address);
                context.Events.Add(newEvent);
                context.SaveChanges();

                return Redirect("/Events");
            }


            List<EventCategory> categories = context.Categories.ToList(); //reload category list options to make sure they will appear after the data validation errors

            return View(new AddEventViewModel(categories, new EventAddress())); //passing new Model Object with categories list options
        }

        private string UploadedFile(AddEventViewModel model)
        {
            string uniqueFileName = null;

            if (model.EventImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.EventImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.EventImage.CopyTo(fileStream);
                }
            }

            else if(model.EventImage == null)
            {
                Console.WriteLine("model.EventImage is Null");
            }

            return uniqueFileName;
        }


        public IActionResult Detail(int id)
        {
           ;
            if (TempData.Peek("EventID") != null) 
            {
                id = (int)TempData["EventID"];
            }

            Console.WriteLine("Events ID: " + id);

            Event theEvent = context.Events
                .Include(e => e.Category)
                .Include(e => e.Address)
                .Single(e => e.Id == id);

            List<EventTag> eventTags = context.EventTags
              .Where(et => et.EventId == id)
              .Include(et => et.Tag)
              .ToList();

            EventDetailViewModel viewModel = new EventDetailViewModel(theEvent, eventTags);

            return View(viewModel);
        }


        public IActionResult Edit(int Id)
        {
            // Pull the Event object that will be edited from the database
            Event theEvent = context.Events.Find(Id);
            EventAddress theAddress = context.Addresses.Find(theEvent.AddressId);

            //Make sure category list will show up 
            List<EventCategory> categories = context.Categories.ToList();

            //Update addEventViewModel fields before returning it to the Edit View
            AddEventViewModel addEventViewModel = new AddEventViewModel(categories, theAddress)
            { 
                Name = theEvent.Name,
                Description = theEvent.Description,
                ContactEmail = theEvent.ContactEmail,
                CategoryId = theEvent.CategoryId,
                eventAddress = theAddress,
                EventId = Id
            };

            return View(addEventViewModel);

        }

        [HttpPost]
        public IActionResult Edit(AddEventViewModel addEventViewModel)
        {

            if (ModelState.IsValid)
            {

                EventCategory theCategory = context.Categories.Find(addEventViewModel.CategoryId);

                Event theEvent = context.Events.Find(addEventViewModel.EventId);

                EventAddress theAddress = context.Addresses.Find(addEventViewModel.eventAddress.Id);


                //Update the Address
                theAddress.Street = addEventViewModel.eventAddress.Street;
                theAddress.City = addEventViewModel.eventAddress.City;
                theAddress.State = addEventViewModel.eventAddress.State;
                theAddress.Zipcode = addEventViewModel.eventAddress.Zipcode;
                theAddress.Latitude = addEventViewModel.eventAddress.Latitude;
                theAddress.Longitude = addEventViewModel.eventAddress.Longitude;
               
                //Update the Event 
                theEvent.Name = addEventViewModel.Name;
                theEvent.Description = addEventViewModel.Description;
                theEvent.ContactEmail = addEventViewModel.ContactEmail;
                theEvent.Category = theCategory;
                theEvent.Address = theAddress;
                   


                context.Events.Update(theEvent);
                context.SaveChanges();

                TempData.Add("EventID",addEventViewModel.EventId);

                return RedirectToAction("Detail");
            }

            //reload and update category list options to make sure they will appear after the data validation errors
            List<EventCategory> categories = context.Categories.ToList(); 
            addEventViewModel.Categories = addEventViewModel.CategoryUpdate(categories);

            return View(addEventViewModel); //passing new Model Object with categories list options

        }


        public IActionResult Delete(int Id)
        {

            Event theEvent = context.Events.Find(Id);
            context.Events.Remove(theEvent);

            context.SaveChanges();
            return Redirect("/Events");
        }


        //Get method to delete multiple events at once
        /*        public IActionResult Delete()
                {
                    ViewBag.events = context.Events.ToList();


                    return View();
                }*/

        //Post method to delete multiple events at once
        /* [HttpPost]
         public IActionResult Delete(int[] eventIds)
         {
             foreach (int eventId in eventIds)
             {
                 //EventData.Remove(eventId);
                 Event theEvent = context.Events.Find(eventId);
                 context.Events.Remove(theEvent);
             }

             context.SaveChanges();
             return Redirect("/Events");
         }*/
    }
}
