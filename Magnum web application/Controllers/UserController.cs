using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Magnum_web_application.Controllers
{
	[Route("api/UserAuth")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IUserRepository _userRepository;
		protected ApiResponse _response;

		public UserController(ApplicationDbContext context, IUserRepository userRepository)
        {
			_context = context;
			_userRepository = userRepository;
			_response = new ApiResponse();
		}

		[HttpPost("Login")]
        public IActionResult Login([FromBody]LoginRequestDTO loginRequestDTO)
		{
			try
			{
				var user = _userRepository.Login(loginRequestDTO);
				
				if (user == null)
				{
					_response.ErrorMessage = "User does not exist";
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					return Ok(_response);
				}

				_response.StatusCode = HttpStatusCode.OK;
				_response.Response = user;
				return Ok(_response);
			}
			catch (Exception)
			{
				_response.ErrorMessage = "Ivalid login";
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				return Ok(_response);
			}
			
		}
	}
}
