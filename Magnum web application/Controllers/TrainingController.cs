using Magnum_web_application.Models;
using Magnum_web_application.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Magnum_web_application.Controllers
{
	[Route("api/Trainings")]
	[ApiController]
	public class TrainingController : ControllerBase
	{
		private readonly ITrainingService trainingService;
		protected ApiResponse apiResponse;

		public TrainingController(ITrainingService trainingService)
		{
			apiResponse = new();
			this.trainingService = trainingService;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult<ApiResponse>> GetSessionsByMemberId(int id, int month = 0)
		{
			apiResponse = await trainingService.GetSessionsByMemberIdAsync(id, month);
			return Ok(apiResponse);
		}

		[HttpGet("SessionsHistory/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSessionsHistory(int id)
		{
			apiResponse = await trainingService.GetSessionHistoryAsync(id);
			return Ok(apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(int memberId)
		{
			apiResponse = await trainingService.CreateSessionAsync(memberId);
			return Ok(apiResponse);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult> Delete(DateTime date)
		{
			apiResponse = await trainingService.DeleteSessionAsync(date);
			return Ok(apiResponse);
		}
	}
}
