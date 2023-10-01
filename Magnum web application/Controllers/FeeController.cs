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
	[Route("api/Fees")]
	[ApiController]
	public class FeeController : ControllerBase
	{
		private readonly IFeeRepository _repository;

		private readonly IMemberRepository _memberRepository;
		private readonly IMapper _mapper;
		private readonly IActiveMemberRepository _activeMemberRepository;
		protected ApiResponse apiResponse;


		public FeeController(IFeeRepository repository, IMemberRepository memberRepository, IMapper mapper, IActiveMemberRepository activeMemberRepository)
		{
			apiResponse = new();
			_repository = repository;
			_memberRepository = memberRepository;
			_mapper = mapper;
			_activeMemberRepository = activeMemberRepository;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task<ActionResult<ApiResponse>> GetFeesByMemberId(int id)
		{
			try
			{
				List<Fee> feeList = await _repository.GetAllAsync(u => u.MemberId == id);

				if (feeList.Count != 0)
				{
					apiResponse.Get(feeList);
					return Ok(apiResponse);
				}

				apiResponse.NotFound(feeList);
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e);
			}
			return Ok(apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> CreateFee(int memberId)
		{
			Member member = await _memberRepository.GetByIdAsync(u => u.Id == memberId);
			if (member == null)
			{
				apiResponse.NotFound(member);
				return Ok(apiResponse);
			}

			FeeDTO feeDTO = new();
			feeDTO.CreateFeeDTO(memberId); 
			Fee model = _mapper.Map<Fee>(feeDTO);

			await _repository.CreateAsync(model);
			await _repository.SaveAsync();

			apiResponse.Create(feeDTO);
			return Ok(apiResponse);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Delete(int id)
		{
			Fee fee = await _repository.GetByIdAsync(u => u.Id == id);
			if (fee == null)
			{
				apiResponse.NotFound(fee);
				return Ok(apiResponse);
			}

			await _repository.DeleteAsync(fee);
			await _repository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;
			return Ok(apiResponse);
		}
	}
}
