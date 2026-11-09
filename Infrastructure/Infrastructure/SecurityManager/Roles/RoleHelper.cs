using Infrastructure.SecurityManager.NavigationMenu;

namespace Infrastructure.SecurityManager.Roles;

public class RoleHelper
{
    public static List<string> GetAdminRoles()
    {
        var roles = new List<string>();
        roles = NavigationTreeStructure.GetCompleteFirstMenuNavigationSegment();
        return roles;
    }

    //make sure or cross check with NavigationTreeStructure
    public static string GetProfileRole()
    {
        return "Profiles";
    }
}
