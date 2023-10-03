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
		ActiveMember addMember = new ActiveMember();

		public ActiveMemberService(IActiveMemberRepository repository, ITrainingSessionRepository trainingSessionRepository)
		{
			this.repository = repository;
			this.apiResponse = new ApiResponse();
			this.trainingSessionRepository = trainingSessionRepository;
		}

		public async Task<ApiResponse> CreateActiveMemberAsync(int id)
		{
			try
			{
				List<ActiveMember> activeMemberByMonths = await repository.GetAllAsync(u => u.MemberId == id);
				List<TrainingSession> trainingSessions = await trainingSessionRepository.GetAllAsync(u => u.MemberId == id);
				List<ActiveMember> listToAdd = new List<ActiveMember>();
				bool isActive;

				var distinctMonths = trainingSessions
					.Select(session => new DateTime(session.SessionDate.Year, session.SessionDate.Month, 1))
					.Distinct();

				foreach (var month in distinctMonths)
				{
					isActive = trainingSessions
						.Count(session => new DateTime(session.SessionDate.Year, session.SessionDate.Month, 1) == month) >= 3;

					if (isActive)
					{
						ActiveMember newMember = new()
						{
							MemberId = id,
							Month = month,
						};

						int memberIdToCheck = id;
						DateTime monthToCheck = month;

						bool containsMember = activeMemberByMonths.Any(member =>
							member.MemberId == memberIdToCheck && member.Month == monthToCheck);

						if (!containsMember)
						{
							listToAdd.Add(newMember);

							await repository.CreateAsync(newMember);
							await repository.SaveAsync();
						}
					}
				}

				if(listToAdd.Count > 0)
				{
					return apiResponse.Create(listToAdd);
				}

				apiResponse.NotFound(addMember);
				apiResponse.ErrorMessage = "Member is not active or is already added to list";
				return apiResponse;
			}
			catch (Exception e)
			{
				return apiResponse.Unauthorize(e);
			}
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
