using AutoMapper.Execution;
using Magnum_Web_App.Models.DTO;

namespace Magnum_Web_App.Services.IServices
{
	public interface IMemberService
	{
		Task<T> GetAllAsync<T>();
		Task<T> GetAsync<T>(int id);
		Task<T> CreateAsync<T>(MemberDTO memberDTO);
		Task<T> UpdateAsync<T>(MemberDTO memberDTO);
		Task<T> DeleteAsync<T>(int id);
	}
}
