using MeetAdl.Data;
using MeetAdl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups.Events
{
    public class CreateModel : PageModel
    {
        private readonly IGroupRepository groupRepository;
        private readonly IEventRepository eventRepository;

        public Group? GroupSummary;

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

        public CreateModel(IGroupRepository groupRepository, IEventRepository eventRepository)
        {
            this.groupRepository = groupRepository;
            this.eventRepository = eventRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId)
        {
            GroupSummary = await groupRepository.GetGroupSummaryAsync(groupId);

            if (GroupSummary == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] long groupId)
        {
            if (!ModelState.IsValid || EventName == null || EventLocation == null || EventDate == null)
            {
                return Page();
            }

            bool groupFound = await groupRepository.CheckIfGroupExistsAsync(groupId);
            if (!groupFound)
            {
                return NotFound();
            }

            MeetUpEvent createdEvent = await eventRepository.CreateEventForGroupAsync(groupId, EventName, EventDescription, EventLocation, EventDate.Value);


            // Send back to detail page
            return RedirectToPage("./details", new { groupId = groupId, eventId = createdEvent.Id });
        }
    }
}
