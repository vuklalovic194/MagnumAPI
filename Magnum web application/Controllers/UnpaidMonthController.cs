using Magnum_web_application.Models;
using Magnum_web_application.Service;
using Magnum_web_application.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Magnum_web_application.Controllers
{
    [Route("api/UnpaidMonth")]
	[ApiController]
	public class UnpaidMonthController : ControllerBase
	{
		private readonly IUnpaidMonthService unpaidMonthService;
		protected ApiResponse apiResponse;

		public UnpaidMonthController(IUnpaidMonthService unpaidMonthService)
		{
			apiResponse = new ApiResponse();
			this.unpaidMonthService = unpaidMonthService;
		}

		[HttpGet("GetAll")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetUnpaidMonths()
		{
			try
			{
				apiResponse = await unpaidMonthService.GetAllUnpaidMonthsAsync();
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
				apiResponse = await unpaidMonthService.GetUnpaidMonthsById(id);
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
