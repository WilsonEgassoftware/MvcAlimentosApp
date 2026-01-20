using SupermarketAPI.DTOs;

namespace SupermarketAPI.Services
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardDataAsync();
    }
}
