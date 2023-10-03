using Magnum_web_application.Models;

namespace Magnum_web_application.Service.IServices
{
	public interface IFeeService
	{
		public Task<ApiResponse> GetFeesByMemberIdAsync(int memberId);
		public Task<ApiResponse> CreateFeeAsync(int memberId);
		public Task<ApiResponse> DeleteFeeAsync(int memberId);
	}
}
