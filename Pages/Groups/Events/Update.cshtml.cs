using MeetAdl.Data;
using MeetAdl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups.Events
{
    public class UpdateModel : PageModel
    {
        private readonly IEventRepository eventRepository;

        [BindProperty]
        public long? EventId { get; set; }
        [BindProperty]
        public long? GroupId { get; set; }

        [BindProperty]
        [Required, MinLength(5)]
        public string? EventName { get; set; }

        [BindProperty]
        public string? EventDescription { get; set; }

        [BindProperty]
        [Required, MinLength(5)]
        public string? EventLocation { get; set; }

        [BindProperty]
        [Required]
        public DateTime? EventDate { get; set; }

        public MeetUpEvent? EventDetails;

        public UpdateModel(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId, [FromQuery] long eventId)
        {
            MeetUpEvent? eventDetails = await eventRepository.GetEventDetailsForGroupAsync(groupId, eventId);

            if (eventDetails == null)
            {
                return NotFound();
            }

            EventId = eventId;
            GroupId = groupId;
            EventName = eventDetails.Name;
            EventDescription = eventDetails.Description;
            EventLocation = eventDetails.Location;
            EventDate = eventDetails.DateTimeAt;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] long groupId, [FromQuery] long eventId)
        {
            if (!ModelState.IsValid || EventName == null || EventLocation == null || EventDate == null)
            {
                return Page();
            }

            await eventRepository.UpdateEventForGroupAsync(groupId, eventId, EventName, EventDescription, EventLocation, EventDate.Value);


            // Send back to detail page
            return RedirectToPage("./details", new { groupId = groupId, eventId = eventId });
        }
    }
}
