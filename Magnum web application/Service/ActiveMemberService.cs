using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.IServices;

namespace Magnum_web_application.Service
{
	public class ActiveMemberService : IActiveMemberService
	{
		private readonly IActiveMemberRepository repository;
		private readonly ITrainingSessionRepository trainingSessionRepository;
		public ApiResponse apiResponse;

		public ActiveMemberService(IActiveMemberRepository repository, ITrainingSessionRepository trainingSessionRepository)
		{
			this.repository = repository;
			this.apiResponse = new ApiResponse();
			this.trainingSessionRepository = trainingSessionRepository;
		}

		public async Task<ApiResponse> GetActiveMemberAsync(int id)
		{
			try
			{
				List<ActiveMember> activeMembers = await repository.GetAllAsync(u => u.MemberId == id);

				if (activeMembers.Count != 0)
				{
					return apiResponse.Get(activeMembers);
				}

				apiResponse.NotFound(activeMembers);
				apiResponse.ErrorMessage = "Member is not active";
				return apiResponse;
			}
			catch (Exception e)
			{
				return apiResponse.Unauthorize(e);
			}
		}

		public async Task<ApiResponse> GetAllActiveMembersAsync()
		{
			try
			{
				List<ActiveMember> activeMembers = await repository.GetAllAsync();

				if (activeMembers.Count != 0)
				{
					return apiResponse.Get(activeMembers);
				}

				return apiResponse.NotFound(activeMembers);
			}
			catch (Exception e)
			{
				return apiResponse.Unauthorize(e);
			}
		}
	}
}
