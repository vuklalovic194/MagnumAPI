using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;

namespace Magnum_web_application.Service.IServices
{
	public interface IMemberService
	{
		public Task<ApiResponse> CreateMemberIfValidAsync(MemberDTO memberDTO);
		public Task<ApiResponse> DeleteMemberAsync(int memberId);
		public Task<ApiResponse> GetMemberByIdAsync(int memberId);
		public Task<ApiResponse> GetMembersAsync();
		public Task<ApiResponse> UpdateMemberAsync(MemberDTO memberDTO, int memberId);
	}
}
