using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.IServices;
using System.Net;

namespace Magnum_web_application.Service
{
    public class TrainingService : ITrainingService
    {
        public ApiResponse apiResponse;
        private readonly ITrainingSessionRepository trainingSessionRepository;

        public TrainingService(ITrainingSessionRepository trainingSessionRepository)
        {
            this.apiResponse = new();
            this.trainingSessionRepository = trainingSessionRepository;
        }

        public async Task<ApiResponse> CreateSessionAsync(int memberId)
        {
            if (await trainingSessionRepository.GetByIdAsync(u => u.MemberId == memberId) == null)
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

        public async Task<ApiResponse> GetSessionHistoryAsync(int memberId)
        {
            List<TrainingSession> trainingSessions = await trainingSessionRepository.GetAllAsync(u => u.MemberId == memberId);

            if (trainingSessions.Count != 0)
            {
                return apiResponse.Get(trainingSessions);
            }
            return apiResponse.NotFound(trainingSessions);
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
