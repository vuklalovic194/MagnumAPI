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
		private readonly IUnpaidMonthRepository unpaidMonthRepository;
		protected ApiResponse apiResponse;


		public FeeController(IFeeRepository repository, IMemberRepository memberRepository, IMapper mapper, IUnpaidMonthRepository unpaidMonthRepository)
		{
			apiResponse = new();
			_repository = repository;
			_memberRepository = memberRepository;
			_mapper = mapper;
			this.unpaidMonthRepository = unpaidMonthRepository;
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
				
				if(id == 0)
				{
					apiResponse.Response = await _repository.GetAllAsync();
					return Ok(apiResponse);
				}

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
			try
			{
				List<UnpaidMonth> unpaidMonths = await unpaidMonthRepository.GetAllAsync(u => u.MemberId == memberId);
				Fee fee = new Fee();
				
				if(unpaidMonths.Count > 0)
				{
					fee = fee.CreateFee(memberId);

					await _repository.CreateAsync(fee);
					await unpaidMonthRepository.DeleteAsync(unpaidMonths[0]);
					await _repository.SaveAsync();

					apiResponse.Create(fee);
					return Ok(apiResponse);
				}
				
				apiResponse.NotFound(fee);
				apiResponse.ErrorMessage = "This member has no debt";
				return Ok(apiResponse);
			}
			catch (Exception e)
			{
				apiResponse.Unauthorize(e) ;
				return Ok(apiResponse);
			}
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
