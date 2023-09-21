﻿using AutoMapper;
using Azure;
using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Magnum_web_application.Controllers
{
	[Route("api/Members")]
	[ApiController]
	public class MemberController : ControllerBase
	{
		private readonly IMemberRepository _repository;
		private readonly IMapper _mapper;
		private readonly ITrainingSessionRepository _trainingSessionRepository;
		private readonly IFeeRepository _feeRepository;
		protected ApiResponse apiResponse;

		public MemberController(IMemberRepository repository, IMapper mapper, 
			ITrainingSessionRepository trainingSessionRepository, IFeeRepository feeRepository)
		{
			apiResponse = new();
			_repository = repository;
			_mapper = mapper;
			_trainingSessionRepository = trainingSessionRepository;
			_feeRepository = feeRepository;
		}

		[HttpGet(Name ="GetMembers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetMembers()
		{
			try
			{
				List<Member> memberList = await _repository.GetAllAsync();
				TrainingSession trainingSession = new TrainingSession();
				Fee fee = new Fee();

				foreach(var member in memberList)
				{
					if (trainingSession.CheckIsTraining())
					{
						fee.CheckIfPaid();
						await _repository.Update(member);
						await _repository.SaveAsync();
					}
				}

				apiResponse.Response = memberList;
				apiResponse.StatusCode = HttpStatusCode.OK;
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				return Ok(apiResponse);
			}
		}

		[HttpGet("{id}", Name = "GetMember")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> GetMember(int id)
		{
			try
			{
				Member member = await _repository.GetByIdAsync(u => u.Id == id);
				if (member == null)
				{
					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NotFound;
					apiResponse.ErrorMessage = "Member not found";

					return Ok(apiResponse);
				}

				apiResponse.StatusCode = HttpStatusCode.OK;
				apiResponse.Response = member;
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				return Ok(apiResponse);
			}
		}

		[HttpPost(Name = "CreateMember")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Create([FromBody] MemberDTO memberDTO)
		{
			try
			{
				if(!ModelState.IsValid)
				{
					apiResponse.IsSuccess = false;
					apiResponse.StatusCode = HttpStatusCode.BadRequest;
					apiResponse.ErrorMessage = "Model State is not valid";
					return Ok(apiResponse);
				}
				if (await _repository.GetByIdAsync(u => u.Name == memberDTO.Name) != null)
				{
					apiResponse.IsSuccess = false;
					apiResponse.StatusCode = HttpStatusCode.BadRequest;
					apiResponse.ErrorMessage = "Member with same name and last name already exists";

					return Ok(apiResponse);
				}

				Member model = _mapper.Map<Member>(memberDTO);
				await _repository.CreateAsync(model);
				await _repository.SaveAsync();

				apiResponse.StatusCode = HttpStatusCode.Created;
				apiResponse.Response = model;
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				return Ok(apiResponse);
			}
		}

		[HttpPut("{id}", Name = "UpdateMember"),]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Update([FromBody] MemberDTO updateDTO, int id)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					apiResponse.IsSuccess = false;
					apiResponse.StatusCode = HttpStatusCode.BadRequest;
					apiResponse.ErrorMessage = "Model State is not valid";
					return Ok(apiResponse);
				}

				Member member = await _repository.GetByIdAsync(u => u.Id == id);
				if (member == null)
				{
					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NotFound;
					apiResponse.ErrorMessage = "Member not found";
					return NotFound();
				}

				updateDTO.mapMember(updateDTO, member);

				await _repository.Update(member);
				await _repository.SaveAsync();

				apiResponse.StatusCode = HttpStatusCode.NoContent;
				apiResponse.Response = member;
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				return Ok(apiResponse);
			}
		}

		[HttpDelete("{id}", Name = "DeleteMember")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Delete(int id)
		{
			try
			{
				Member member = await _repository.GetByIdAsync(u => u.Id == id);
				if (member == null)
				{
					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NotFound;
					apiResponse.ErrorMessage = "Member not found";
					return NotFound();
				}

				await _repository.DeleteAsync(member);
				await _repository.SaveAsync();

				apiResponse.StatusCode = HttpStatusCode.NoContent;
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				return Ok(apiResponse);
			}
		}

		[HttpPut("{id}/ManageSessions", Name ="ManageSessions")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ManageSessions(int id, [FromBody] IsTrainingDTO isTrainingDTO)
		{
			try
			{
				TrainingSession trainingSession = await _trainingSessionRepository.GetByIdAsync(u=> u.MemberId == id);
				if (trainingSession != null)
				{
					await _trainingSessionRepository.AddSession(trainingSession);
					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NoContent;
					apiResponse.Response = trainingSession;
					return Ok(apiResponse);
				}
				else
				{
					TrainingSession trainingSes = new TrainingSession();
					trainingSes.mapForNewSession(isTrainingDTO, id);

					await _trainingSessionRepository.CreateAsync(trainingSes);
					await _trainingSessionRepository.AddSession(trainingSes);
					await _trainingSessionRepository.SaveAsync();

					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.Created;
					apiResponse.Response = trainingSession;
					return Ok(apiResponse);
				}
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				return Ok(apiResponse);
			}
		}

		[HttpPut("{id}/ManageFees", Name ="ManageFees")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ManageFees(int id, [FromBody]IsPaidDTO isPaidDTO)
		{
			try
			{
				Member member = await _repository.GetByIdAsync(u => u.Id == id, tracked: false);
				Fee fee = new Fee();
				TrainingSession trainingSession = new TrainingSession();

				if (member == null)
				{
					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NotFound;
					apiResponse.ErrorMessage = "Member not found";
					return NotFound();
				}
				else
				{
					isPaidDTO.mapPaid(fee, isPaidDTO);
					if (trainingSession.CheckIsTraining())
					{
						fee.CheckIfPaid();
					}

					await _repository.Update(member);
					await _repository.SaveAsync();

					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NoContent;
					apiResponse.Response = member;
					return Ok(apiResponse);
				}
			}
			catch (Exception e)
			{
				apiResponse.StatusCode = HttpStatusCode.Unauthorized;
				apiResponse.ErrorMessage = new string(e.Message.ToString());
				apiResponse.IsSuccess = false;
				return Ok(apiResponse);
			}
		}
	}
}
