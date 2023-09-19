using Magnum_web_application.Models;

namespace Magnum_web_application.Repository.IRepository
{
	public interface ITrainingSessionRepository : IRepository<TrainingSession>
	{
		Task<TrainingSession> Update(TrainingSession training);
	}
}
