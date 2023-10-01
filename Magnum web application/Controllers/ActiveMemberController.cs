using AutoMapper;
using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace Magnum_web_application.Controllers
{
	[Route("api/ActiveMembers")]
	[ApiController]
	public class ActiveMembers : ControllerBase
	{
		private readonly IActiveMemberRepository _activeMemberRepository;
		private readonly ITrainingSessionRepository _trainingSessionRepository;
		private readonly ApiResponse _apiResponse;
		ActiveMember addMember = new ActiveMember();

		public ActiveMembers(IActiveMemberRepository activeMemberRepository, ITrainingSessionRepository trainingSessionRepository)
		{
			_activeMemberRepository = activeMemberRepository;
			_trainingSessionRepository = trainingSessionRepository;
			_apiResponse = new();
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetActiveMembers()
		{
			try
			{
				List<ActiveMember> activeMembers = await _activeMemberRepository.GetAllAsync();

				if (activeMembers.Count != 0)
				{
					_apiResponse.Get(activeMembers);
					return Ok(_apiResponse);
				}
				_apiResponse.NotFound(activeMembers);
				return Ok(_apiResponse);
			}
			catch (Exception e)
			{
				_apiResponse.Unauthorize(e);
			}
			return Ok(_apiResponse);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetActiveMember(int id)
		{
			try
			{
				List<ActiveMember> activeMembers = await _activeMemberRepository.GetAllAsync( u => u.MemberId == id);

				if (activeMembers.Count != 0)
				{
					_apiResponse.Get(activeMembers);
					return Ok(_apiResponse);
				}
				_apiResponse.NotFound(activeMembers);
				_apiResponse.ErrorMessage = "Member is not active";
				return Ok(_apiResponse);
			}
			catch (Exception e)
			{
				_apiResponse.Unauthorize(e);
			}
			return Ok(_apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> CreateActiveMembers(int id)
		{
			try
			{
				List<ActiveMember> activeMembers = await _activeMemberRepository.GetAllAsync(u => u.MemberId == id);
				List<TrainingSession> trainingSessions = await _trainingSessionRepository.GetAllAsync(u => u.MemberId == id);
				
				List<ActiveMember> listToAdd = addMember.AddToActiveMember(trainingSessions, activeMembers, id);

				if (listToAdd.Count > 0)
				{
					foreach (ActiveMember ac in listToAdd)
					{
						await _activeMemberRepository.CreateAsync(ac);
						await _activeMemberRepository.SaveAsync();
					}
					_apiResponse.Create(listToAdd);
					return Ok(_apiResponse);
				}

				_apiResponse.NotFound(addMember);
				_apiResponse.ErrorMessage = "Member is not active";
				return Ok(_apiResponse);
			}
			catch (Exception e)
			{
				_apiResponse.Unauthorize(e);
			}
			return Ok(_apiResponse);
		}
	}
}
