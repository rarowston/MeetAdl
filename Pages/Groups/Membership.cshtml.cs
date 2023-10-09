
using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Permissions;
using MeetAdl.Permissions.Requirements;
using MeetAdl.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups;
[AuthorizeForGroupAccess(PermissionLevel.ReadGroupMembers)]
public class MembershipModel : PageModel
{
    private readonly ICurrentIdentityService currentIdentityService;
    private readonly IUserRepository userRepository;
    private readonly IGroupRepository groupRepository;

    /// <summary>
    /// The Permission Level represented by this edit model. On Set this will force set all of the selectors to match the provided permission level
    /// On Get this will retrieve an Administrator role representing the current selection of permission levels
    /// </summary>
    [BindProperty]
    public PermissionLevel PermissionLevel
    {
        get
        {
            return GeneratePermissionLevel();
        }
        set
        {
            InitialisePermissionSelectors(value);
        }
    }

    public User? ProfileDetails;
    public Group? GroupDetails;

    public bool PageForCurrentUser { get; set; } = false;

    [BindProperty]
    public List<PermissionLevelSelector> RoleSelectors { get; set; }

    public MembershipModel(ICurrentIdentityService currentIdentityService, IUserRepository userRepository, IGroupRepository groupRepository)
    {
        this.currentIdentityService = currentIdentityService;
        this.userRepository = userRepository;
        this.groupRepository = groupRepository;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] long? groupId, [FromQuery] long? userId)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return RedirectToPage("~/Index");
        }

        if (groupId == null)
        {
            return RedirectToPage("~/Index");
        }

        if (userId == null)
        {
            return RedirectToPage("Details", new { groupId = groupId });
        }

        // Retrieve the information about any user. 
        ProfileDetails = await userRepository.GetUserFromUserIdAsync(userId.Value);
        GroupDetails = await groupRepository.GetGroupSummaryAsync(groupId.Value);

        GroupMember? groupMember = await userRepository.GetUserAccessToGroupAsync(userId.Value, groupId.Value);

        if (ProfileDetails == null || groupMember == null)
        {
            return NotFound();
        }
        else
        {
            // Check if page for the current user
            if (currentIdentityService.ObjectId == (ProfileDetails.ObjectId))
            {
                PageForCurrentUser = true;
            }
            else
            {
                PageForCurrentUser = false;
            }

            PermissionLevel = groupMember.UserGroupPermissions;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync([FromQuery] long? groupId, [FromQuery] long? userId)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return RedirectToPage("~/Index");
        }

        if (groupId == null)
        {
            return RedirectToPage("~/Index");
        }

        if (userId == null)
        {
            return RedirectToPage("Details", new { groupId = groupId });
        }

        if (!await currentIdentityService.CurrentUserCanAccessGroupAsync(groupId.Value, PermissionLevel.EditGroupMembers))
        {
            return Unauthorized();
        }

        await userRepository.UpdateGroupMembershipPermissionsAsync(groupId.Value, userId.Value, PermissionLevel);
        return RedirectToPage("Details", new { groupId = groupId });
    }



    /// <summary>
    /// Initialises the role selector lists with all current roles defined in the enums with some extra rules applied. 
    /// The RoleSelectors list will be filled with all simple permission levels. 
    /// The Preset roles list will be filled with all composite roles except those containing write. 
    /// </summary>
    private void InitialiseRoleSelectorList()
    {
        if (RoleSelectors == null)
        {
            RoleSelectors = new List<PermissionLevelSelector>();
        }
        else
        {
            RoleSelectors.Clear();
        }

        foreach (PermissionLevel role in Enum.GetValues(typeof(PermissionLevel)))
        {
            // Deliberately ignore standard access as that is the absence of permissions
            if (role == PermissionLevel.Authenticated)
            {
                continue;
            }

            // Only add single role flags to this list
            if ((role & role - 1) == 0)
            {
                RoleSelectors.Add(new PermissionLevelSelector(role));
            }
        }
    }

    /// <summary>
    /// Sets the initial values for all permission selectors based on an existing permission level
    /// </summary>
    /// <param name="permissions">The set of permissions to load into the selection checkboxes</param>
    private void InitialisePermissionSelectors(PermissionLevel permissions)
    {
        if (RoleSelectors == null)
        {
            InitialiseRoleSelectorList();
        }

        // Set initial values for all individual Permissions
        foreach (PermissionLevelSelector roleSelection in RoleSelectors!)
        {
            if (permissions.HasFlag(roleSelection.Role))
            {
                roleSelection.IsSelected = true;
            }
            else
            {
                roleSelection.IsSelected = false;
            }
        }
    }


    /// <summary>
    /// Generates the permission level based on the checkboxes that are selected
    /// </summary>
    private PermissionLevel GeneratePermissionLevel()
    {
        PermissionLevel calculatedRole = PermissionLevel.Authenticated;
        foreach (PermissionLevelSelector roleSelection in RoleSelectors)
        {
            if (roleSelection.IsSelected)
            {
                calculatedRole |= roleSelection.Role;
            }
        }
        return calculatedRole;
    }
}

public class PermissionLevelSelector
{
    public PermissionLevelSelector()
    {

    }

    public PermissionLevelSelector(PermissionLevel role)
    {
        Role = role;
        Name = role.ToString();
    }

    public PermissionLevel Role { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
}

