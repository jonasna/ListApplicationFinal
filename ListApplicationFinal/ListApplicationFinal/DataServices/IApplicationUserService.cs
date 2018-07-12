using System.Threading.Tasks;

namespace ListApplicationFinal.DataServices
{
    public interface IApplicationUserService
    {
        string DisplayName { get; set; }
        string Name { get; set; }
        bool IsValid { get; }
        Task SaveUserDataAsync();
        Task ClearAsync();
    }
}