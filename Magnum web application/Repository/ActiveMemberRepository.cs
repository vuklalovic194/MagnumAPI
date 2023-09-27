using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Magnum_web_application.Repository
{
	public class ActiveMemberRepository : Repository<ActiveMember>, IActiveMemberRepository
	{
		private readonly ApplicationDbContext _context;

		public ActiveMemberRepository(ApplicationDbContext context) : base(context)
        {
			_context = context;
		}

		public async Task<ActiveMember> Update(ActiveMember activeMember)
		{
			_context.Update(activeMember);
			_context.SaveChanges();
			return activeMember;
		}
	}
}
