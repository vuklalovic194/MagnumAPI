using Magnum_web_application.Models;

namespace Magnum_web_application.Service.IServices
{
	public interface ITrainingService
	{
		public Task<ApiResponse> CreateSessionAsync(int memberId);
		public Task<ApiResponse> DeleteSessionAsync(DateTime date);
		public Task<ApiResponse> GetSessionHistoryAsync(int memberId);
		public Task<ApiResponse> GetSessionsByMemberIdAsync(int memberId, int month = 0);
	}
}
