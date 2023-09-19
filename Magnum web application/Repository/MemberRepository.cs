using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Repository
{
	public class MemberRepository : Repository <Member>, IMemberRepository 
	{
		private readonly ApplicationDbContext _context;

		public MemberRepository(ApplicationDbContext context) : base(context) 
        {
			_context = context;
		}

		public async Task<Member> Update(Member entity)
		{
			entity.DateUpdated = DateTime.UtcNow;
			_context.Update(entity);
			_context.SaveChanges();
			return entity;
		}
	}
}
