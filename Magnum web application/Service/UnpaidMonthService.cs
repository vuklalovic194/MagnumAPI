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
