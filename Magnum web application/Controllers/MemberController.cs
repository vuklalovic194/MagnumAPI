using AutoMapper;
using Azure;
using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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

				if(memberList.Count <= 0) 
				{
					apiResponse.NotFound(memberList);
					return Ok(apiResponse);
				}
				
				apiResponse.Get(memberList);
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
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
					apiResponse.NotFound(member);
					return Ok(apiResponse);
				}

				apiResponse.Get(member);
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
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
					apiResponse.BadRequest(memberDTO);
					return Ok(apiResponse);
				}
				else if (await _repository.GetByIdAsync(u => u.Name == memberDTO.Name) != null)
				{
					apiResponse.BadRequest(memberDTO);
					apiResponse.ErrorMessage = "Member with same name already exists";
					return Ok(apiResponse);
				}

				Member model = _mapper.Map<Member>(memberDTO);
				await _repository.CreateAsync(model);
				await _repository.SaveAsync();

				apiResponse.Create(model);
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
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
					apiResponse.BadRequest(updateDTO);
					return Ok(apiResponse);
				}

				Member member = await _repository.GetByIdAsync(u => u.Id == id);
				if (member == null)
				{
					apiResponse.NotFound(member);
					return NotFound();
				}

				updateDTO.mapMember(updateDTO, member);
				await _repository.Update(member);
				await _repository.SaveAsync();

				apiResponse.Update(member);
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
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
					apiResponse.NotFound(member);
					return Ok(apiResponse);
				}

				await _repository.DeleteAsync(member);
				await _repository.SaveAsync();

				apiResponse.StatusCode = HttpStatusCode.NoContent;
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
				return Ok(apiResponse);
			}
		}
	}
}
