using Magnum_Web_App.Models;

namespace Magnum_Web_App.Services.IServices
{
	public interface ITrainingService : IBaseServices
	{
		Task<T> GetAllAsync<T>();
		Task<T> GetAsync<T>(int id);
		Task<T> CreateAsync<T>(TrainingSession training);
		Task<T> UpdateAsync<T>(TrainingSession training);
		Task<T> DeleteAsync<T>(int id);
	}
}
