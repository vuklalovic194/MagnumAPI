using Magnum_web_application.Models;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;
using System.Net;

namespace Magnum_web_application.Service.TrainingService
{
	public class DeleteSessionService
	{
		public ApiResponse apiResponse;
		private readonly ITrainingSessionRepository trainingSessionRepository;

		public DeleteSessionService(ITrainingSessionRepository trainingSessionRepository, ApiResponse apiResponse)
		{
			this.apiResponse = apiResponse;
			this.trainingSessionRepository = trainingSessionRepository;
		}

		public async Task<ApiResponse> DeleteSessionAsync(DateTime date)
		{
			TrainingSession trainingSession = await trainingSessionRepository.GetByIdAsync(u => u.SessionDate == date);
			if (trainingSession == null)
			{
				return apiResponse.NotFound(trainingSession);
			}

			await trainingSessionRepository.DeleteAsync(trainingSession);
			await trainingSessionRepository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;
			return apiResponse;
		}
	}
}
