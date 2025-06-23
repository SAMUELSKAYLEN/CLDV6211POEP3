using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10443307CLVDProjectPOEPart3.Models;

namespace ST10443307CLVDProjectPOEPart3.Controllers
{
    public class EventController : Controller
    {
        private readonly EventEaseContext _context;

        public EventController(EventEaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchType, int? VENUEID, DateTime? startDate, DateTime? endDate)
        {
            var Event = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
                Event = Event.Where(e => e.EventType.EventTypeName == searchType);

            if (VENUEID.HasValue)
                Event = Event.Where(e => e.VENUEID == VENUEID);

            if (startDate.HasValue && endDate.HasValue)
                Event = Event.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);

            //provide data for dropdown filters in view
            ViewData["Venues"] = _context.Venues.ToList();
            ViewData["EventType"] = _context.EventType.ToList();
            return View(await Event.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Venues = _context.Venues.ToList();
            //Load eventTypes for dropdown list
            ViewBag.EventType = _context.EventType.ToList();
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event Event)
        {

            if (ModelState.IsValid)
            {
                _context.Add(Event);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event created successfully. ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Venues = _context.Venues.ToList();
            //reload event types on failed form submit
            ViewBag.EventType = _context.EventType.ToList();
            return View(Event);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var Event = await _context.Events.FirstOrDefaultAsync(m => m.EVENTID == id);

            if (Event == null)
            {
                return NotFound();
            }
            return View(Event);
        }
        //Confirm deletion
        public async Task<IActionResult> Delete(int? id) 
        {
            if(id ==null) return NotFound();

            var @Event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(n => n.EVENTID == id);

            if (@Event == null)
            {
                return NotFound();
            }
            return View(@Event);
        }
        //Perfrom deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id) 
        {
            var @Event = await _context.Events.FindAsync(id);
            if (@Event == null) return NotFound();

            var isBooked = await _context.Bookings.AnyAsync(b => b.EVENTID == id);
            if (isBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete event because it already exists in bookings";
                return RedirectToAction(nameof(Index));
            }

            _ = _context.Events.Remove(@Event);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event deleted successfully. ";
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id) 
        { 
            return _context.Events.Any(m => m.EVENTID == id);
        }

        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null) 
            {
                return NotFound();
            }

            var Event = await _context.Events.FindAsync(id);
   
            if (Event == null)
            {
                return NotFound();
            }
            ViewBag.Venues = _context.Venues.ToList();
            //load eventType for dropdown
            ViewBag.EventType = _context.EventType.ToList();
            return View(@Event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event Event) 
        {
            if (id != Event.EVENTID) 
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
            }
            try
            {
                _context.Update(Event);
                await _context.SaveChangesAsync();
            }

            catch(DbUpdateConcurrencyException)
            {
                if (!EventExists(Event.EVENTID))
                {
                    return NotFound();
                }
                else 
                {
                    throw;
                }
            }
                return RedirectToAction(nameof(Index));

        }

    }
}
