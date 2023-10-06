using Magnum_web_application.Models;

namespace Magnum_web_application.Service.IServices
{
	public interface IActiveMemberService
	{
		public Task<ApiResponse> GetAllActiveMembersAsync();
		public Task<ApiResponse> GetActiveMemberAsync(int id);
	}
}
