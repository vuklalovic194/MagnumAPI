using Magnum_web_application.Models;

namespace Magnum_web_application.Service.IServices
{
	public interface IUnpaidMonthService
	{
		public Task<ApiResponse> GetAllUnpaidMonthsAsync();
		public Task<ApiResponse> GetUnpaidMonthsById(int memberId);
	}
}
