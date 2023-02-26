using System.Threading.Tasks;

namespace coderush.Services
{
    public interface IRoles
    {
        Task GenerateRolesFromPagesAsync();

        Task AddToRoles(string applicationUserId);
    }
}
