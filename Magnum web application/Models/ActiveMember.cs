using Magnum_web_application.Controllers;

namespace Magnum_web_application.Models
{
	public class ActiveMember
	{
		public int Id { get; set; }
		public DateTime Month { get; set; }

		public Member Member { get; set; }
		public int MemberId { get; set; }

		public List<ActiveMember> AddToActiveMember(List<TrainingSession> trainingSessions, List<ActiveMember> activeMembers, int id) 
		{
			List<ActiveMember> listToAdd = new List<ActiveMember>();
			bool isActive;

			var distinctMonths = trainingSessions
				.Select(session => new DateTime(session.SessionDate.Year, session.SessionDate.Month, 1))
				.Distinct();

			foreach (var month in distinctMonths)
			{
				isActive = trainingSessions
					.Count(session => new DateTime(session.SessionDate.Year, session.SessionDate.Month, 1) == month) >= 3;

				if (isActive == true)
				{
					ActiveMember mem = new();
					mem.MemberId = id;
					mem.Month = month;

					int memberIdToCheck = id;
					DateTime monthToCheck = month;

					bool containsMember = activeMembers.Any(member =>
						member.MemberId == memberIdToCheck && member.Month == monthToCheck);

					if (containsMember == false && isActive)
					{
						listToAdd.Add(mem);
						return listToAdd;
						//await _activeMemberRepository.CreateAsync(mem);
						//await _activeMemberRepository.SaveAsync();
					}
				}
			}
			return listToAdd;
		}
	}
}
