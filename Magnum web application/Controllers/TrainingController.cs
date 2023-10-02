using AutoMapper;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.TrainingService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Magnum_web_application.Controllers
{
	[Route("api/Trainings")]
	[ApiController]
	public class TrainingController : ControllerBase
	{
		private readonly GetSessionsByMemberIdService getSessionsByMemberIdService;
		private readonly GetSessionHistoryService getSessionHistoryService;
		private readonly DeleteSessionService deleteSessionService;
		protected ApiResponse apiResponse;


		public TrainingController(GetSessionsByMemberIdService getSessionsByMemberIdService, GetSessionHistoryService getSessionHistoryService, DeleteSessionService deleteSessionService)
		{
			apiResponse = new();
			this.getSessionsByMemberIdService = getSessionsByMemberIdService;
			this.getSessionHistoryService = getSessionHistoryService;
			this.deleteSessionService = deleteSessionService;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult<ApiResponse>> GetSessionsByMemberId(int id, int month = 0)
		{
			apiResponse = await getSessionsByMemberIdService.GetSessionsByMemberIdAsync(id, month);
			return Ok(apiResponse);
		}

		[HttpGet("SessionsHistory/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSessionsHistory(int id)
		{
			apiResponse = await getSessionHistoryService.GetSessionHistoryAsync(id);
			return Ok(apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(int memberId)
		{
			apiResponse = await getSessionHistoryService.GetSessionHistoryAsync(memberId);
			return Ok(apiResponse);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult> Delete(DateTime date)
		{
			apiResponse = await deleteSessionService.DeleteSessionAsync(date);
			return Ok(apiResponse);
		}
	}
}
