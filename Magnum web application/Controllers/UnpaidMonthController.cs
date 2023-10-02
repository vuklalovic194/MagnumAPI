using Magnum_web_application.Models;
using Magnum_web_application.Service.UnpaidMonthService;
using Microsoft.AspNetCore.Mvc;

namespace Magnum_web_application.Controllers
{
	[Route("api/UnpaidMonth")]
	[ApiController]
	public class UnpaidMonthController : ControllerBase
	{
		private readonly CreateUnpaidMonthService createUnpaidMonthService;
		private readonly GetUnpaidMonthByMemberIdService unpaidMonthServiceById;
		private readonly GetAllUnpaidMonthsService unpaidMonthsService;
		protected ApiResponse apiResponse;

		public UnpaidMonthController(
			CreateUnpaidMonthService unpaidMonthService,
			GetUnpaidMonthByMemberIdService unpaidMonthServiceById,
			GetAllUnpaidMonthsService unpaidMonthsService)
		{

			apiResponse = new ApiResponse();
			this.createUnpaidMonthService = unpaidMonthService;
			this.unpaidMonthServiceById = unpaidMonthServiceById;
			this.unpaidMonthsService = unpaidMonthsService;
		}

		[HttpGet("GetAll")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetUnpaidMonths()
		{
			try
			{
				apiResponse = await unpaidMonthsService.GetAllUnpaidMonthsAsync();
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
				return Ok(apiResponse);
			}
		}

		[HttpGet("Get/{id}")]
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

		[HttpPost("Create")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> CreateUnpaidMonth(int memberId)
		{
			try
			{
				apiResponse = await createUnpaidMonthService.CreateUnpaidMonth(memberId);
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
