using Magnum_web_application.Models.DTO;

namespace Magnum_web_application.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUnique(string username);
		Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
	}
}
