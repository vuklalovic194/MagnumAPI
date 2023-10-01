using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Repository
{
	public class UnpaidMonthRepository : Repository<UnpaidMonth>, IUnpaidMonthRepository
	{
		private readonly ApplicationDbContext _context;

		public UnpaidMonthRepository(ApplicationDbContext context) : base(context)
        {
			_context = context;
		}
    }
}
