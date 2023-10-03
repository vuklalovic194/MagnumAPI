using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.IServices;

namespace Magnum_web_application.Service
{
    public class UnpaidMonthService : IUnpaidMonthService
    {
        private readonly IUnpaidMonthRepository unpaidMonthRepository;
        private readonly IActiveMemberRepository activeMemberRepository;
        private readonly IFeeRepository feeRepository;
        private ApiResponse apiResponse;

        List<DateTime> listOfMonth = new List<DateTime>();
        List<DateTime> listOfFees = new List<DateTime>();

        public UnpaidMonthService(
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
            List<ActiveMember> activeMonths = await activeMemberRepository.GetAllAsync(u => u.MemberId == memberId);
            List<UnpaidMonth> unpaidMonths = await unpaidMonthRepository.GetAllAsync();
            List<Fee> fees = await feeRepository.GetAllAsync(u => u.MemberId == memberId);
            UnpaidMonth unpaidMonth = new UnpaidMonth();

            try
            {
                for (int i = 0; i < activeMonths.Count; i++)
                {
					listOfMonth.Add(activeMonths[i].Month);
                }
				for (int j = 0; j < fees.Count; j++)
				{
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
						await unpaidMonthRepository.SaveAsync();
					}
				}
                return apiResponse.Create(unpaidMonth);
            }
            catch (Exception e)
            {
                return apiResponse.NotFound(e);
            }
        }

        public async Task<ApiResponse> GetAllUnpaidMonthsAsync()
        {
            List<UnpaidMonth> unpaidMonths = await unpaidMonthRepository.GetAllAsync();

            if (unpaidMonths.Count != 0)
            {
                apiResponse.Get(unpaidMonths);
                return apiResponse;
            }
            apiResponse.NotFound(unpaidMonths);
            return apiResponse;
        }

        public async Task<ApiResponse> GetUnpaidMonthsById(int memberId)
        {
            List<UnpaidMonth> unpaidMonths = await unpaidMonthRepository.GetAllAsync(u => u.MemberId == memberId);
            if (unpaidMonths.Count != 0)
            {
                apiResponse.Get(unpaidMonths);
                return apiResponse;
            }

            apiResponse.NotFound(unpaidMonths);
            return apiResponse;
        }
    }
}
