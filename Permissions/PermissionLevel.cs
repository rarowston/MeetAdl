namespace MeetAdl.Permissions;

public enum PermissionLevel : long // Ensure that the enum is 64 bits by making it a long - cannot use ulong as db does not support it. 
{
    Authenticated = 0, // No elevated access - any authenticated user. Value: 0

    ReadGroupDetails = ((long)1) << 0,
    EditGroupDetails = ((long)1) << 1,
    DeleteGroups = ((long)1) << 2,

    ReadGroupEvents = ((long)1) << 4,
    EditGroupEvents = ((long)1) << 5,
    CancelGroupEvents = ((long)1) << 6,

    CreateGroupPosts = ((long)1) << 8,
    EditGroupPosts = ((long)1) << 9,
    DeleteGroupPosts = ((long)1) << 10,

    ReadGroupMembers = ((long)1) << 12,
    EditGroupMembers = ((long)1) << 13,

    ReadUserDetails = ((long)1) << 32,
    EditUserDetails = ((long)1) << 33,

    ReadUserPermissions = ((long)1) << 34,
    EditUserPermissions = ((long)1) << 35,

    GroupRead = 
        ReadGroupDetails |
        ReadGroupEvents |
        ReadGroupMembers,

    GroupEdit = 
        EditGroupDetails | 
        EditGroupEvents | 
        EditGroupPosts |
        EditGroupMembers,

    GroupAdvancedEdit = 
        GroupEdit |
        CancelGroupEvents |
        DeleteGroupPosts,

    GroupAdministrate =
        GroupRead | 
        GroupAdvancedEdit |
        DeleteGroups,

    UserRead = 
        ReadUserDetails |
        ReadUserPermissions,

    UserEdit =
        EditUserDetails |
        EditUserPermissions,

    UserAdministrate = 
        UserRead |
        UserEdit,

    GlobalRead = 
        UserRead |
        GroupRead,

    GlobalAdmin = // 64424523639
        UserAdministrate |
        GroupAdministrate
}
