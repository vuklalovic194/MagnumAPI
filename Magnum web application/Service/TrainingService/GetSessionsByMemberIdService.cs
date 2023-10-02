using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using System;

namespace Magnum_web_application.Service.TrainingService
{
	public class GetSessionsByMemberIdService
	{
		public ApiResponse apiResponse;
		private readonly ITrainingSessionRepository trainingSessionRepository;

		public GetSessionsByMemberIdService(ApiResponse apiResponse, ITrainingSessionRepository trainingSessionRepository)
		{
			this.apiResponse = apiResponse;
			this.trainingSessionRepository = trainingSessionRepository;
		}

		public async Task<ApiResponse> GetSessionsByMemberIdAsync(int memberId, int month = 0)
		{
			List<TrainingSession> trainingSession = await trainingSessionRepository.GetAllAsync(u => u.MemberId == memberId);
			if (trainingSession.Count != 0)
			{
				if (month != 0)
				{
					trainingSession = await trainingSessionRepository.GetAllAsync(u => u.SessionDate.Month == month && u.MemberId == memberId);

					return apiResponse.Get(trainingSession.Count);
				}

				return apiResponse.Get(trainingSession.Count);
			}

			return apiResponse.NotFound(trainingSession);
		}
	}
}
