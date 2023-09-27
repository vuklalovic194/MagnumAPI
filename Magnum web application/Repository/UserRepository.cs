using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Magnum_web_application.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;
		private string _secretKey;

		public UserRepository(ApplicationDbContext context, IConfiguration configuration)
        {
			_context = context;
			_secretKey = configuration.GetValue<string>("ApiSettings:Secret");
		}

        public bool IsUnique(string username)
		{
			var user = _context.Users.FirstOrDefault(u=> u.Name == username);

			if(user == null)
			{
				return true;
			}
			
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			User user = _context.Users.FirstOrDefault(u => u.Name == loginRequestDTO.Username && u.Password == loginRequestDTO.Password);
			if(user == null)
			{
				return null;
			}

			//create JWT

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_secretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Name.ToString()),
				}),
				
				Expires = DateTime.UtcNow.AddMinutes(60),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
			{
				Token = tokenHandler.WriteToken(token),
				User = user,
			};

			return loginResponseDTO;
		}
	}
}
