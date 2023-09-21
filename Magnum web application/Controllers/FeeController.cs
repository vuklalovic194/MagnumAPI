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
		protected ApiResponse apiResponse;


		public FeeController(IFeeRepository repository, IMapper mapper)
		{
			apiResponse = new();
			_repository = repository;
			_mapper = mapper;
			
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[Authorize]
		public async Task <ActionResult<ApiResponse>> GetFees()
		{
			try
			{
				List<Fee> memberList = await _repository.GetAllAsync();
				apiResponse.Response = memberList;
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
		public async Task <ActionResult<ApiResponse>> GetFee(int id)
		{
			Fee fee = await _repository.GetByIdAsync(u => u.Id == id);
			if (fee == null)
			{
				apiResponse.IsSuccess = true;
				apiResponse.StatusCode=HttpStatusCode.NotFound;
				apiResponse.ErrorMessage = "Member not found";

				return Ok(apiResponse);
			}

			apiResponse.StatusCode = HttpStatusCode.OK;
			apiResponse.Response = fee;
			return Ok(apiResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Create([FromBody]Fee fee)
		{
			if (await _repository.GetByIdAsync(u => u.Id == fee.Id) != null)
            {
				apiResponse.IsSuccess = false;
				apiResponse.StatusCode=HttpStatusCode.BadRequest;
				apiResponse.ErrorMessage = "Fee with same id already exists";
				
				return Ok(apiResponse);
            }

            await _repository.CreateAsync(fee);
			await _repository.SaveAsync();

			apiResponse.StatusCode=HttpStatusCode.Created;
			apiResponse.Response = fee;
			return Ok(apiResponse);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Update([FromBody] Fee fee, int id)
		{
			Fee feeFromDb = await _repository.GetByIdAsync(u=>u.Id == id);
			if(feeFromDb == null)
			{
				apiResponse.IsSuccess = true;
				apiResponse.StatusCode = HttpStatusCode.NotFound;
				apiResponse.ErrorMessage = "Fee not found";
				return NotFound();
			}

			feeFromDb.IsPaid = fee.IsPaid;
			feeFromDb.Debt = fee.Debt;
			feeFromDb.DatePaid = DateTime.UtcNow;

			await _repository.Update(feeFromDb);
			await _repository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;
			apiResponse.Response = fee;
			return Ok(apiResponse);
			
		}

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
				apiResponse.ErrorMessage = "Fee not found";
				return NotFound();
			}

			await _repository.DeleteAsync(fee);
			await _repository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;

			return Ok(apiResponse);
		}
	}
}
