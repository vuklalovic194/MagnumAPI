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

		private readonly IMapper _mapper;
		private readonly IMemberRepository _memberRepository;
		protected ApiResponse apiResponse;


		public FeeController(IFeeRepository repository, IMapper mapper, IMemberRepository memberRepository)
		{
			apiResponse = new();
			_repository = repository;
			_mapper = mapper;
			_memberRepository = memberRepository;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task <ActionResult<ApiResponse>> GetFeesByMemberId(int id, int month = 0)
		{
			try
			{
				if (month != 0)
				{
					List<Fee> feeByMonth = await _repository.GetAllAsync(u => u.MemberId == id && u.DatePaid.Month == month);

					if (feeByMonth.Count == 0)
					{
						apiResponse.StatusCode = HttpStatusCode.NotFound;
						apiResponse.ErrorMessage = "No payments were proccessed at this specific month";
						apiResponse.IsSuccess = true;

						return Ok(apiResponse);
					}
					
					apiResponse.Response = feeByMonth;
					apiResponse.StatusCode = HttpStatusCode.OK;

					return Ok(apiResponse);
				}

				List<Fee> feeList = await _repository.GetAllAsync(u => u.MemberId == id);

				if(feeList.Count == 0)
				{
					apiResponse.StatusCode = HttpStatusCode.NotFound;
					apiResponse.ErrorMessage = "No payments were processed by this member";
					apiResponse.IsSuccess = true;
					return Ok(apiResponse);
				}

				apiResponse.Response = feeList;
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

		//[HttpGet("{id}")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//public async Task <ActionResult<ApiResponse>> GetFee(int id)
		//{
		//	Fee fee = await _repository.GetByIdAsync(u => u.Id == id);
		//	if (fee == null)
		//	{
		//		apiResponse.IsSuccess = true;
		//		apiResponse.StatusCode=HttpStatusCode.NotFound;
		//		apiResponse.ErrorMessage = "Member not found";

		//		return Ok(apiResponse);
		//	}

		//	apiResponse.StatusCode = HttpStatusCode.OK;
		//	apiResponse.Response = fee;
		//	return Ok(apiResponse);
		//}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> CreateFee(int memberId)
		{
			Member member = await _memberRepository.GetByIdAsync(u => u.Id == memberId);
			if (member == null)
            {
				apiResponse.IsSuccess = false;
				apiResponse.StatusCode=HttpStatusCode.BadRequest;
				apiResponse.ErrorMessage = "There is no member with that id";
				
				return Ok(apiResponse);
            }

			Fee fee = new()
			{
				MemberId = memberId,
				DatePaid = DateTime.Now,
			};

            await _repository.CreateAsync(fee);
			await _repository.SaveAsync();

			apiResponse.StatusCode=HttpStatusCode.Created;
			apiResponse.Response = fee;
			return Ok(apiResponse);
		}

		//[HttpPut("{id}")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//public async Task<ActionResult> Update([FromBody] Fee fee, int id)
		//{
		//	Fee feeFromDb = await _repository.GetByIdAsync(u=>u.Id == id);
		//	if(feeFromDb == null)
		//	{
		//		apiResponse.IsSuccess = true;
		//		apiResponse.StatusCode = HttpStatusCode.NotFound;
		//		apiResponse.ErrorMessage = "Fee not found";
		//		return NotFound();
		//	}

		//	//feeFromDb.IsPaid = fee.IsPaid;
		//	//feeFromDb.Debt = fee.Debt;
		//	feeFromDb.DatePaid = DateTime.UtcNow;

		//	await _repository.Update(feeFromDb);
		//	await _repository.SaveAsync();

		//	apiResponse.StatusCode = HttpStatusCode.NoContent;
		//	apiResponse.Response = fee;
		//	return Ok(apiResponse);
			
		//}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task <ActionResult> Delete(int id)
		{
			Fee fee = await _repository.GetByIdAsync(u=> u.Id == id);
			if(fee == null)
			{
				apiResponse.IsSuccess = true;
				apiResponse.StatusCode = HttpStatusCode.NotFound;
				apiResponse.ErrorMessage = "Payment not found";
				return NotFound();
			}

			await _repository.DeleteAsync(fee);
			await _repository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;

			return Ok(apiResponse);
		}
	}
}
