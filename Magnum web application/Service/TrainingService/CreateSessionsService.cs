using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Service.TrainingService
{
	public class CreateSessionsService
	{
		public ApiResponse apiResponse;
		private readonly ITrainingSessionRepository trainingSessionRepository;

		public CreateSessionsService(ApiResponse apiResponse, ITrainingSessionRepository trainingSessionRepository)
		{
			this.apiResponse = apiResponse;
			this.trainingSessionRepository = trainingSessionRepository;
		}

		public async Task<ApiResponse> CreateSessionAsync(int memberId)
		{
			if (await trainingSessionRepository.GetByIdAsync(u => u.Id == memberId) == null)
			{
				return apiResponse.BadRequest(memberId);
			}

			TrainingSession trainingSession = new()
			{
				MemberId = memberId,
				SessionDate = DateTime.UtcNow
			};

			await trainingSessionRepository.CreateAsync(trainingSession);
			await trainingSessionRepository.SaveAsync();

			return apiResponse.Create(trainingSession);
		}
	}
}
