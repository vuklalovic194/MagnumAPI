using Magnum_web_application.Models;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Magnum_web_application.Controllers
{
	[Route("api/UnpaidMonth")]
	[ApiController]
	public class UnpaidMonthController : ControllerBase
	{
		private readonly IUnpaidMonthRepository _unpaidMonthRepository;
		private readonly IActiveMemberRepository activeMemberRepository;
		private readonly IFeeRepository _feeRepository;
		protected ApiResponse apiResponse;

		public UnpaidMonthController(IUnpaidMonthRepository unpaidMonthRepository, IActiveMemberRepository activeMemberRepository, IFeeRepository feeRepository)
		{
			_unpaidMonthRepository = unpaidMonthRepository;
			this.activeMemberRepository = activeMemberRepository;
			_feeRepository = feeRepository;
			apiResponse = new ApiResponse();
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetUnpaidMonths()
		{
			List<UnpaidMonth> unpaidMonths = await _unpaidMonthRepository.GetAllAsync();
			if (unpaidMonths.Count != 0)
			{
				apiResponse.Get(unpaidMonths);
				return Ok(apiResponse);
			}

			apiResponse.NotFound(unpaidMonths);
			return Ok(apiResponse);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetUnpaidMonthsByMemberId(int id)
		{
			List<UnpaidMonth> unpaidMonths = await _unpaidMonthRepository.GetAllAsync(u => u.MemberId == id);
			if (unpaidMonths.Count != 0)
			{
				apiResponse.Get(unpaidMonths);
				return Ok(apiResponse);
			}

			apiResponse.NotFound(unpaidMonths);
			return Ok(apiResponse);
		}

		[HttpPost]
		public async Task<IActionResult> CreateUnpaidMonth(int memberId)
		{
			try
			{
				List<ActiveMember> activeMembers = await activeMemberRepository.GetAllAsync(u => u.MemberId == memberId);
				List<UnpaidMonth> unpaidMonths = await _unpaidMonthRepository.GetAllAsync();
				List<Fee> fees = await _feeRepository.GetAllAsync(u => u.MemberId == memberId);
				UnpaidMonth unpaidMonth = new UnpaidMonth();

				if (activeMembers.Count != 0)
				{
					List<UnpaidMonth> list = unpaidMonth.CreateUnpaidMonth(activeMembers, fees, unpaidMonths, memberId);

					if(list.Count != 0)
					{
						foreach (var item in list)
							await _unpaidMonthRepository.CreateAsync(item);

						await _unpaidMonthRepository.SaveAsync();

						apiResponse.StatusCode = HttpStatusCode.Created;
						return Ok(apiResponse);
					}
				}

				apiResponse.ErrorMessage = "There are no unpaid months";
				apiResponse.IsSuccess = true;
				apiResponse.StatusCode = HttpStatusCode.NotFound;
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
				return Ok(apiResponse);
			}
		}
	}
}
