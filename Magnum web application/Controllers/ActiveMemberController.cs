using Magnum_web_application.Models;
using Magnum_web_application.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Magnum_web_application.Controllers
{
	[Route("api/ActiveMembers")]
	[ApiController]
	public class ActiveMembers : ControllerBase
	{
		private readonly IActiveMemberService activeMemberService;
		public ApiResponse apiResponse;
		
		public ActiveMembers(IActiveMemberService activeMemberService)
		{
			apiResponse = new();
			this.activeMemberService = activeMemberService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetActiveMembers()
		{
			apiResponse = await activeMemberService.GetAllActiveMembersAsync();
			return Ok(apiResponse);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetActiveMember(int id)
		{
			apiResponse = await activeMemberService.GetActiveMemberAsync(id);
			return Ok(apiResponse);
		}
	}
}
