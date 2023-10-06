using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;

namespace Magnum_web_application.Service.IServices
{
	public interface IRankService
	{
		Task<ApiResponse> GetAllRanksAsync(int memberId);
		Task<ApiResponse> GetRankAsync(int memberId);
		Task<ApiResponse> CreateRankAsync(RankDTO rankDTO, int id);
	}
}
