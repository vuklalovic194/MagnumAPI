using AutoMapper.Execution;
using Magnum_web_application.Controllers;

namespace Magnum_web_application.Models
{
	public class UnpaidMonth
	{
		public int Id { get; set; }
		public DateTime Month { get; set; }

		public Member Member { get; set; }
		public int MemberId { get; set; }

		List<DateTime> listOfMonth = new List<DateTime>();
		List<DateTime> listOfFees = new List<DateTime>();
		List<UnpaidMonth> monthsToAdd = new List<UnpaidMonth>();

		public List<UnpaidMonth> CreateUnpaidMonth
			(List<ActiveMember> activeMembers,
			List<Fee> fees,
			List<UnpaidMonth> unpaidMonths,
			int memberId)
		{
			foreach (var month in activeMembers)
			{
				listOfMonth.Add(month.Month);
			}
			foreach (var fee in fees)
			{
				listOfFees.Add(fee.DatePaid);
			}

			var unmatchedMonths = listOfMonth.Except(listOfFees).ToList();

			foreach (var date in unmatchedMonths)
			{
				if (!unpaidMonths.Any(unpaid => unpaid.Month == date && unpaid.MemberId == memberId))
				{
					UnpaidMonth addUnpaidMonth = new()
					{
						Month = date,
						MemberId = memberId,
					};

					monthsToAdd.Add(addUnpaidMonth);
				}
			}
			return monthsToAdd;
		}
	}
}
