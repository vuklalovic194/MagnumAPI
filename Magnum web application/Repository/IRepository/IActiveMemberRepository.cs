using Magnum_web_application.Models;

namespace Magnum_web_application.Repository.IRepository
{
	public interface IActiveMemberRepository : IRepository<ActiveMember>
	{
		Task<ActiveMember> Update(ActiveMember activeMember);
	}
}
