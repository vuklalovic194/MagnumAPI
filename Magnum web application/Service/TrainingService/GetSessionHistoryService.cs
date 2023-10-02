using Magnum_web_application.Models;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Service.TrainingService
{
	public class GetSessionHistoryService
	{
		public ApiResponse apiResponse;
		private readonly ITrainingSessionRepository trainingSessionRepository;

		public GetSessionHistoryService(ApiResponse apiResponse, ITrainingSessionRepository trainingSessionRepository)
		{
			this.apiResponse = apiResponse;
			this.trainingSessionRepository = trainingSessionRepository;
		}

		public async Task<ApiResponse> GetSessionHistoryAsync(int id)
		{
			List<TrainingSession> trainingSessions = await trainingSessionRepository.GetAllAsync(u => u.MemberId == id);

			if (trainingSessions.Count != 0)
			{
				return apiResponse.Get(trainingSessions);
			}
			return apiResponse.NotFound(trainingSessions);
		}
	}
}
