using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Service.UnpaidMonthService
{
	public class CreateUnpaidMonthService
	{
		private readonly IUnpaidMonthRepository unpaidMonthRepository;
		private readonly IActiveMemberRepository activeMemberRepository;
		private readonly IFeeRepository feeRepository;
		private ApiResponse apiResponse;

		List<DateTime> listOfMonth = new List<DateTime>();
		List<DateTime> listOfFees = new List<DateTime>();

		public CreateUnpaidMonthService(
			IUnpaidMonthRepository unpaidMonthRepository,
			IActiveMemberRepository activeMemberRepository,
			IFeeRepository feeRepository
			)
		{
			this.unpaidMonthRepository = unpaidMonthRepository;
			this.activeMemberRepository = activeMemberRepository;
			this.feeRepository = feeRepository;
			apiResponse = new();
		}

		public async Task<ApiResponse> CreateUnpaidMonth(int memberId)
		{
			List<ActiveMember> activeMembers = await activeMemberRepository.GetAllAsync(u => u.MemberId == memberId);
			List<UnpaidMonth> unpaidMonths = await unpaidMonthRepository.GetAllAsync();
			List<Fee> fees = await feeRepository.GetAllAsync(u => u.MemberId == memberId);
			UnpaidMonth unpaidMonth = new UnpaidMonth();

			try
			{
				for (int i = 0, j = 0; i < activeMembers.Count && j < fees.Count; i++, j++)
				{
					listOfMonth.Add(activeMembers[i].Month);
					listOfFees.Add(fees[j].DatePaid);
				}

				var unmatchedMonths = listOfMonth.Except(listOfFees).ToList();

				foreach (var date in unmatchedMonths)
				{
					if (!unpaidMonths.Any(unpaid => unpaid.Month == date && unpaid.MemberId == memberId))
					{
						unpaidMonth.Month = date;
						unpaidMonth.MemberId = memberId;

						await unpaidMonthRepository.CreateAsync(unpaidMonth);
					}
				}
				await unpaidMonthRepository.SaveAsync();
				return apiResponse.Create(unpaidMonth);
			}
			catch (Exception e)
			{
				return apiResponse.NotFound(e);
			}
		}
	}
}
