using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Service.UnpaidMonthService
{
	public class GetUnpaidMonthByMemberIdService
	{
		private readonly IUnpaidMonthRepository unpaidMonthRepository;
		public ApiResponse apiResponse;

		public GetUnpaidMonthByMemberIdService(IUnpaidMonthRepository unpaidMonthRepository)
		{
			this.unpaidMonthRepository = unpaidMonthRepository;
			this.apiResponse = new ApiResponse();
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
