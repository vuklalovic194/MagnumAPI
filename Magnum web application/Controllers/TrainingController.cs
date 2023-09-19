using AutoMapper;
using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Magnum_web_application.Controllers
{
	[Route("api/Trainings")]
	[ApiController]
	public class TrainingController : ControllerBase
	{
		private readonly ITrainingSessionRepository _repository;
		private readonly IMapper _mapper;
		protected ApiResponse apiResponse;


		public TrainingController(ITrainingSessionRepository repository, IMapper mapper)
		{
			apiResponse = new();
			_repository = repository;
			_mapper = mapper;
			
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task <ActionResult<ApiResponse>> GetMembers()
		{
			try
			{
				List<TrainingSession> trainingSession = await _repository.GetAllAsync();
				apiResponse.Response = trainingSession;
				apiResponse.StatusCode = HttpStatusCode.OK;
				
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				
			}
			return Ok(apiResponse);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult<ApiResponse>> GetMember(int id)
		{
			TrainingSession trainingSession = await _repository.GetByIdAsync(u => u.Id == id);
			if (trainingSession == null)
			{
				apiResponse.IsSuccess = true;
				apiResponse.StatusCode=HttpStatusCode.NotFound;
				apiResponse.ErrorMessage = "Member not found";

				return Ok(apiResponse);
			}

			apiResponse.StatusCode = HttpStatusCode.OK;
			apiResponse.Response = trainingSession;
			return Ok(apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Create([FromBody] TrainingSession trainingSession)
		{
			if (await _repository.GetByIdAsync(u => u.Id == trainingSession.Id) != null)
            {
				apiResponse.IsSuccess = false;
				apiResponse.StatusCode=HttpStatusCode.BadRequest;
				apiResponse.ErrorMessage = "Training Session with same id already exists";
				
				return Ok(apiResponse);
            }

			//Member model = _mapper.Map<Member>(memberDTO);

            await _repository.CreateAsync(trainingSession);
			await _repository.SaveAsync();

			apiResponse.StatusCode=HttpStatusCode.Created;
			apiResponse.Response = trainingSession;
			return Ok(apiResponse);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Update([FromBody] TrainingSession trainingSession, int id)
		{
			TrainingSession trainingSessionFromDb = await _repository.GetByIdAsync(u=>u.Id == id);
			if(trainingSession == null)
			{
				apiResponse.IsSuccess = true;
				apiResponse.StatusCode = HttpStatusCode.NotFound;
				apiResponse.ErrorMessage = "Member not found";
				return NotFound();
			}

			trainingSessionFromDb = trainingSession;

			await _repository.Update(trainingSessionFromDb);
			await _repository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;
			apiResponse.Response = trainingSessionFromDb;
			return Ok(apiResponse);
			
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult> Delete(int id)
		{
			TrainingSession trainingSession = await _repository.GetByIdAsync(u=> u.Id == id);
			if(trainingSession == null)
			{
				apiResponse.IsSuccess = true;
				apiResponse.StatusCode = HttpStatusCode.NotFound;
				apiResponse.ErrorMessage = "trainingSession not found";
				return NotFound();
			}

			await _repository.DeleteAsync(trainingSession);
			await _repository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;

			return Ok(apiResponse);
		}
	}
}
