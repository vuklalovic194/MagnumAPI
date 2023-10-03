using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.IServices;
using System.Net;

namespace Magnum_web_application.Service
{
	public class FeeService : IFeeService
	{
		public ApiResponse apiResponse;
		private readonly IFeeRepository repository;
		private readonly IUnpaidMonthRepository unpaidMonthRepository;

		public FeeService(
			IFeeRepository repository,
			IUnpaidMonthRepository unpaidMonthRepository)
		{
			this.apiResponse = new ApiResponse();
			this.repository = repository;
			this.unpaidMonthRepository = unpaidMonthRepository;
		}

		public async Task<ApiResponse> CreateFeeAsync(int memberId)
		{
			try
			{
				List<UnpaidMonth> unpaidMonths = await unpaidMonthRepository.GetAllAsync(u => u.MemberId == memberId);
				Fee fee = new Fee();

				if (unpaidMonths.Count > 0)
				{
					fee = fee.CreateFee(memberId);

					await repository.CreateAsync(fee);
					await unpaidMonthRepository.DeleteAsync(unpaidMonths[0]);
					await repository.SaveAsync();

					return apiResponse.Create(fee);
				}

				apiResponse.NotFound(fee);
				apiResponse.ErrorMessage = "This member has no debt";
				return apiResponse;
			}
			catch (Exception e)
			{
				return apiResponse.Unauthorize(e);
			}
		}

		public async Task<ApiResponse> DeleteFeeAsync(int memberId)
		{
			try
			{
				List<Fee> fees = await repository.GetAllAsync(u => u.MemberId == memberId);
				if (fees.Count <= 0)
				{
					return apiResponse.NotFound(fees);
				}

				await repository.DeleteAsync(fees.Last());
				await repository.SaveAsync();

				apiResponse.StatusCode = HttpStatusCode.NoContent;
				return apiResponse;
			}
			catch (Exception e)
			{
				return apiResponse.Unauthorize(e);
			}
		}
		
		public async Task<ApiResponse> GetFeesByMemberIdAsync(int memberId)
		{
			try
			{
				List<Fee> feeList = await repository.GetAllAsync(u => u.MemberId == memberId);

				if (memberId == 0)
				{
					apiResponse.Response = await repository.GetAllAsync();
					return apiResponse;
				}

				if (feeList.Count != 0)
				{
					return apiResponse.Get(feeList);
				}

				return apiResponse.NotFound(feeList);
			}
			catch (Exception e)
			{
				return apiResponse.Unauthorize(e);
			}
		}
	}
}
