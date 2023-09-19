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
		protected ApiResponse apiResponse;

		public MemberController(IMemberRepository repository, IMapper mapper)
		{
			apiResponse = new();
			_repository = repository;
			_mapper = mapper;
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

				foreach(var member in memberList)
				{
					if (member.CheckIsTraining())
					{
						member.CheckIfPaid();
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
				if(ModelState.IsValid)
				{
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
				else
				{
					apiResponse.IsSuccess = false;
					apiResponse.StatusCode = HttpStatusCode.BadRequest;
					apiResponse.ErrorMessage = "Model State is not valid";
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

		[HttpPut("{id}", Name = "UpdateMember"),]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Update([FromBody] MemberDTO updateDTO, int id)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Member member = await _repository.GetByIdAsync(u => u.Id == id);
					if (member == null)
					{
						apiResponse.IsSuccess = true;
						apiResponse.StatusCode = HttpStatusCode.NotFound;
						apiResponse.ErrorMessage = "Member not found";
						return NotFound();
					}

					member.Address = updateDTO.Address;
					member.Name = updateDTO.Name;
					member.Rank = updateDTO.Rank;
					member.ImageUrl = updateDTO.ImageUrl;
					member.PhoneNumber = updateDTO.PhoneNumber;
					member.Age = updateDTO.Age;

					await _repository.Update(member);
					await _repository.SaveAsync();

					apiResponse.StatusCode = HttpStatusCode.NoContent;
					apiResponse.Response = member;
					return Ok(apiResponse);
				}
				else
				{
					apiResponse.IsSuccess = false;
					apiResponse.StatusCode = HttpStatusCode.BadRequest;
					apiResponse.ErrorMessage = "Model State is not valid";
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
		public async Task<IActionResult> ManageSessions(int id, [FromBody]IsActiveDTO isActiveDTO)
		{
			try
			{
				Member member = await _repository.GetByIdAsync(u => u.Id == id, tracked: false);
				AddSessionDTO memberDTO = new AddSessionDTO();

				if (member == null)
				{
					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NotFound;
					apiResponse.ErrorMessage = "Member not found";
					return Ok(apiResponse);
				}
				else
				{
					memberDTO.isActive = isActiveDTO.IsTraining;
					memberDTO.MonthlySessions = member.MonthlySessions;
					memberDTO.TotalSessions = member.TotalSessions;
					memberDTO.AddSession();

					member.SessionDate = memberDTO.SessionDate;
					member.MonthlySessions = memberDTO.MonthlySessions;
					member.TotalSessions = memberDTO.TotalSessions;

					await _repository.Update(member);
					await _repository.SaveAsync();

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

		[HttpPut("{id}/ManageFees", Name ="ManageFees")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ManageFees(int id, [FromBody]IsPaidDTO isPaidDTO)
		{
			try
			{
				Member member = await _repository.GetByIdAsync(u => u.Id == id, tracked: false);
				AddSessionDTO memberDTO = new AddSessionDTO();

				if (member == null)
				{
					apiResponse.IsSuccess = true;
					apiResponse.StatusCode = HttpStatusCode.NotFound;
					apiResponse.ErrorMessage = "Member not found";
					return NotFound();
				}
				else
				{
					memberDTO.IsPaid = isPaidDTO.isPaid;
					memberDTO.MonthlySessions = member.MonthlySessions;
					memberDTO.SessionDate = member.SessionDate;
					memberDTO.DatePaid = member.DatePaid;
					memberDTO.Debt = member.Debt;

					if (memberDTO.CheckIsTraining())
					{
						memberDTO.CheckIfPaid();
					}

					member.isTraining = memberDTO.isTraining;
					member.MonthlySessions = memberDTO.MonthlySessions;
					member.SessionDate = memberDTO.SessionDate;
					member.DatePaid = memberDTO.DatePaid;
					member.Debt = memberDTO.Debt;

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