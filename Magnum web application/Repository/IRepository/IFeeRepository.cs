using Magnum_web_application.Models;

namespace Magnum_web_application.Repository.IRepository
{
	public interface IFeeRepository : IRepository<Fee>
	{
		Task<Fee> Update(Fee fee);
	}
}
