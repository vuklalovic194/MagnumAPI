using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Magnum_web_application.Repository
{
	public class TrainingSessionRepository : Repository<TrainingSession>, ITrainingSessionRepository
	{
		private readonly ApplicationDbContext _context;

		public TrainingSessionRepository(ApplicationDbContext context) : base(context)
        {
			_context = context;
		}

        public async Task<TrainingSession> AddSession(TrainingSession training)
		{
			//training.AddSingleSession();
			_context.Update(training);
			_context.SaveChanges();
			return training;
		}
	}
}
