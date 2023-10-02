using AutoMapper.Execution;
using Magnum_web_application.Models;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;
using System.Collections.Generic;
using System.Net;

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
		List<UnpaidMonth> monthsToAdd = new List<UnpaidMonth>();



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

			if (monthsToAdd.Count != 0)
			{
				foreach (var item in monthsToAdd)
					await unpaidMonthRepository.CreateAsync(item);

				await unpaidMonthRepository.SaveAsync();
				return apiResponse.Create(unpaidMonth);
			}

			return apiResponse.BadRequest(unpaidMonth);
		}

	}
}
