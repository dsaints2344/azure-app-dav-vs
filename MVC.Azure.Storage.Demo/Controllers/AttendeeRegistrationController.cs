using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Azure.Storage.Demo.Models.Data;
using MVC.Azure.Storage.Demo.Services;

namespace MVC.Azure.Storage.Demo.Controllers
{
    public class AttendeeRegistrationController : Controller
    {

        private readonly ITableStorageService _tableStorageService;

        public AttendeeRegistrationController(ITableStorageService tableStorageService)
        {
            this._tableStorageService = tableStorageService;
        }

        // GET: AttendeeRegistrationController
        public async Task<ActionResult> Index()
        {
            var data = await _tableStorageService.GetAttendees();
            return View(data);
        }

        // GET: AttendeeRegistrationController/Details/5
        public async Task<ActionResult> Details(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);
            return View(data);
        }

        // GET: AttendeeRegistrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendeeRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AttendeeEntity attendeeEntity)
        {
            try
            {
                attendeeEntity.PartitionKey = attendeeEntity.Industry;
                attendeeEntity.RowKey = Guid.NewGuid().ToString();

                await _tableStorageService.UpsertAttendee(attendeeEntity);  
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendeeRegistrationController/Edit/5
        public async Task<ActionResult> Edit(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);
            return View(data);
        }

        // POST: AttendeeRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AttendeeEntity attendeeEntity)
        {
            try
            {
                attendeeEntity.PartitionKey = attendeeEntity.Industry;

                await _tableStorageService.UpsertAttendee(attendeeEntity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: AttendeeRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, string industry)
        {
            try
            {
                await _tableStorageService.DeleteAttendee(industry, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
