using Magnum_Web_App.Models;

namespace Magnum_Web_App.Services.IServices
{
	public interface IFeeService : IBaseServices
	{
		Task<T> GetAllAsync<T>();
		Task<T> GetAsync<T>(int id);
		Task<T> CreateAsync<T>(Fee fee);
		Task<T> UpdateAsync<T>(Fee fee);
		Task<T> DeleteAsync<T>(int id);
	}
}
