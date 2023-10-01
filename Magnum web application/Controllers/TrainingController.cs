using AutoMapper;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Magnum_web_application.Controllers
{
	[Route("api/Trainings")]
	[ApiController]
	public class TrainingController : ControllerBase
	{
		private readonly ITrainingSessionRepository _repository;
		private readonly IMemberRepository _memberRepository;
		protected ApiResponse apiResponse;


		public TrainingController(ITrainingSessionRepository repository, IMemberRepository memberRepository)
		{
			apiResponse = new();
			_repository = repository;
			_memberRepository = memberRepository;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult<ApiResponse>> GetSessionsByMemberId(int id, int month = 0)
		{
			List <TrainingSession> trainingSession = await _repository.GetAllAsync(u => u.MemberId == id);
			if (trainingSession.Count != 0)
			{
				if (month != 0)
				{
					trainingSession = await _repository.GetAllAsync(u => u.SessionDate.Month == month && u.MemberId == id);
					
					apiResponse.Get(trainingSession.Count);
					return Ok(apiResponse);
				}

				apiResponse.Get(trainingSession.Count);
				return Ok(apiResponse);
			}

			apiResponse.NotFound(trainingSession);
			return Ok(apiResponse);
		}

		[HttpGet("SessionsHistory/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSessionsHistory(int id)
		{
			List<TrainingSession> trainingSessions = await _repository.GetAllAsync(u => u.MemberId == id);
			
			if(trainingSessions.Count != 0)
			{
				apiResponse.Get(trainingSessions);
				return Ok(apiResponse);
			}
			apiResponse.NotFound(trainingSessions);
			return Ok(apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(int memberId)
		{
			if(await _memberRepository.GetByIdAsync(u => u.Id == memberId) == null)
			{
				apiResponse.BadRequest(memberId);
				return Ok(apiResponse);
			}

			TrainingSession trainingSession = new()
			{
				MemberId = memberId,
				SessionDate = DateTime.UtcNow
			};

            await _repository.CreateAsync(trainingSession);
			await _repository.SaveAsync();

			apiResponse.Create(trainingSession);
			return Ok(apiResponse);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult> Delete(DateTime date)
		{
			TrainingSession trainingSession = await _repository.GetByIdAsync(u=> u.SessionDate == date);
			if(trainingSession == null)
			{
				apiResponse.NotFound(trainingSession);
				return NotFound();
			}

			await _repository.DeleteAsync(trainingSession);
			await _repository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;
			return Ok(apiResponse);
		}
	}
}
