using Magnum_web_application.Models;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.UnpaidMonthService;
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
		private readonly CreateUnpaidMonthService unpaidMonthService;
		private readonly GetUnpaidMonthByMemberIdService unpaidMonthServiceById;
		protected ApiResponse apiResponse;

		public UnpaidMonthController(IUnpaidMonthRepository unpaidMonthRepository, IActiveMemberRepository activeMemberRepository, IFeeRepository feeRepository, CreateUnpaidMonthService unpaidMonthService, GetUnpaidMonthByMemberIdService unpaidMonthServiceById)
		{

			apiResponse = new ApiResponse();
			this.unpaidMonthService = unpaidMonthService;
			this.unpaidMonthServiceById = unpaidMonthServiceById;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetUnpaidMonths()
		{
			//transfer to service

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
			try
			{
				apiResponse = await unpaidMonthServiceById.GetUnpaidMonthsById(id);
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
				return Ok(apiResponse);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateUnpaidMonth(int memberId)
		{
			try
			{
				apiResponse = await unpaidMonthService.CreateUnpaidMonth(memberId);
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
