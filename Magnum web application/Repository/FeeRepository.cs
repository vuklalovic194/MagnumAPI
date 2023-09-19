using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Repository
{
	public class FeeRepository : Repository<Fee>, IFeeRepository
	{
		private readonly ApplicationDbContext _context;

		public FeeRepository(ApplicationDbContext context) : base(context)
        {
			_context = context;
		}

        public async Task<Fee> Update(Fee fee)
		{
			fee.DatePaid = DateTime.Now;
			_context.Update(fee);
			_context.SaveChanges();
			return fee;
		}
	}
}
