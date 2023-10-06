using Magnum_web_application.Models;
using Magnum_web_application.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Magnum_web_application.Controllers
{
	[Route("api/Fees")]
	[ApiController]
	public class FeeController : ControllerBase
	{
		private readonly IFeeService feeService;
		protected ApiResponse apiResponse;

		public FeeController(IFeeService feeService)
		{
			apiResponse = new();
			this.feeService = feeService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetFeesByMemberId(int id)
		{
			apiResponse = await feeService.GetFeesByMemberIdAsync(id);
			return Ok(apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> CreateFee(int memberId)
		{
			apiResponse = await feeService.CreateFeeAsync(memberId);
			return Ok(apiResponse);
		}

		//skinuti delete
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> Delete(int id)
		{
			apiResponse = await feeService.DeleteFeeAsync(id);
			return Ok(apiResponse);
		}
	}
}
