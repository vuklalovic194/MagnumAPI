using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Service.UnpaidMonthService
{
	public class GetAllUnpaidMonthsService
	{
		private readonly IUnpaidMonthRepository unpaidMonthRepository;
		public ApiResponse apiResponse;

		public GetAllUnpaidMonthsService(IUnpaidMonthRepository unpaidMonthRepository)
		{
			this.unpaidMonthRepository = unpaidMonthRepository;
			apiResponse = new ApiResponse();
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
	}
}

