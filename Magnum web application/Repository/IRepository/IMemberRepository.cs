using Magnum_web_application.Models;

namespace Magnum_web_application.Repository.IRepository
{
	public interface IMemberRepository : IRepository<Member>
	{
		Task<Member> Update(Member entity);
	}
}
