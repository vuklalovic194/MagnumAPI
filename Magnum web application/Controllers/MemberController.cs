using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Magnum_web_application.Controllers
{
    [Route("api/Members")]
	[ApiController]
	public class MemberController : ControllerBase
	{
		private readonly IMemberService memberService;
		protected ApiResponse apiResponse;

		public MemberController(IMemberService memberService)
		{
			apiResponse = new();
			this.memberService = memberService;
		}

		[HttpGet(Name ="GetMembers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetMembers()
		{
			apiResponse = await memberService.GetMembersAsync();
			return Ok(apiResponse);
		}

		[HttpGet("{id}", Name = "GetMember")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> GetMember(int id)
		{
			apiResponse = await memberService.GetMemberByIdAsync(id);
			return Ok(apiResponse);
		}

		[HttpPost(Name = "CreateMember")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Create([FromBody] MemberDTO memberDTO)
		{
			if(ModelState.IsValid)
			{
				apiResponse = await memberService.CreateMemberIfValidAsync(memberDTO);
				return Ok(apiResponse);
			}
			return Ok(apiResponse.BadRequest(memberDTO));
		}

		[HttpPut("{id}", Name = "UpdateMember")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Update([FromBody] MemberDTO updateDTO, int id)
		{
			if (ModelState.IsValid)
			{
				apiResponse = await memberService.UpdateMemberAsync(updateDTO, id);
			}
			return Ok(apiResponse.BadRequest(updateDTO));
		}

		[HttpDelete("{id}", Name = "DeleteMember")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Delete(int id)
		{
			apiResponse = await memberService.DeleteMemberAsync(id);
			return Ok(apiResponse);
		}
	}
}
