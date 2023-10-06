using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Magnum_web_application.Controllers
{
	[Route("api/Rank")]
	[ApiController]
	public class RankController : ControllerBase
	{
		public ApiResponse apiResponse;
		private readonly IRankService rankService;

		public RankController(
			IRankService rankService)
		{
			this.apiResponse = new();
			this.rankService = rankService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllRanksByMemberId(int memberId)
		{
			apiResponse = await rankService.GetAllRanksAsync(memberId);
			return Ok(apiResponse);
		}

		[HttpGet("{memberId}")]
		public async Task<IActionResult> GeRankByMemberId(int memberId)
		{
			apiResponse = await rankService.GetRankAsync(memberId);
			return Ok(apiResponse);
		}

		[HttpPost]
		public async Task<IActionResult> CreateRank([FromBody] RankDTO rankDTO, int memberId)
		{
			apiResponse = await rankService.CreateRankAsync(rankDTO, memberId);
			return Ok(apiResponse);
		}
	}
}
