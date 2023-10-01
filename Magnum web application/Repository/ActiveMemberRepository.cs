using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Magnum_web_application.Repository
{
	public class ActiveMemberRepository : Repository<ActiveMember>, IActiveMemberRepository
	{
		private readonly ApplicationDbContext _context;

		public ActiveMemberRepository(ApplicationDbContext context /* ITrainingSessionRepository trainingSessionRepository*/) : base(context)
        {
			_context = context;
		}

		public async Task<List<ActiveMember>> GroupAndCreateActiveMembers(List<ActiveMember> activeMembers,
			List<TrainingSession> trainingSessions, ActiveMember activeMember)
		{
			var month = DateTime.UtcNow.Month - 1;
			var activeMembersInInt = trainingSessions
				.Where(s => s.SessionDate.Month == month)
				.GroupBy(s => s.MemberId)
				.Where(g => g.Count() >= 3)
				.Select(g => g.Key)
				.ToList();

			foreach (var ac in activeMembersInInt)
			{
				activeMember.MemberId = ac;

				if(activeMembers.Count > 0)
				{
					foreach (var a in activeMembers)
					{
						if (a.MemberId != activeMember.MemberId)
						{
							activeMembers.Add(activeMember);
							_context.Update(activeMember);
							await _context.SaveChangesAsync();
						}
					}
				}
				else if(activeMembers.Count == 0)
				{
					activeMembers.Add(activeMember);
					_context.Update(activeMember);
					await _context.SaveChangesAsync();
				}
			}
			return activeMembers;
		}
	}
}
