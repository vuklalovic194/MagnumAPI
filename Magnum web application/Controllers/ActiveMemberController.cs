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
	[Route("api/ActiveMembers")]
	[ApiController]
	public class ActiveMembers : ControllerBase
	{
		private readonly IActiveMemberRepository _activeMemberRepository;
		private readonly ITrainingSessionRepository _trainingSessionRepository;
		private readonly ApiResponse _apiResponse;

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
				List<TrainingSession> trainingSessions = await _trainingSessionRepository.GetAllAsync();
				List<ActiveMember> activeMembers = new();
				
				var month = DateTime.UtcNow.Month - 1;
				var activeMembersInInt = trainingSessions
					.Where(s => s.SessionDate.Month == month)
					.GroupBy(s => s.MemberId)
					.Where(g => g.Count() >= 3)
					.Select(g => g.Key)
					.ToList();

				foreach (var ac in activeMembersInInt)
				{
					ActiveMember activeMember = new()
					{
						MemberId = ac
					};

					activeMembers.Add(activeMember);

					await _activeMemberRepository.CreateAsync(activeMember);
					await _activeMemberRepository.SaveAsync();
				}

				activeMembers = await _activeMemberRepository.GetAllAsync();

				_apiResponse.Get(activeMembers);
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
