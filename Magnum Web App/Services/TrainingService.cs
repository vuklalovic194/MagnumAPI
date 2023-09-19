using Magnum_Web_App.Models;
using Magnum_Web_App.Services.IServices;
using MagnumApp_Utility;
using Microsoft.Extensions.Configuration;

namespace Magnum_Web_App.Services
{
	public class TrainingService : BaseService, ITrainingService
	{
		private string MagnumApiUrl;

		public TrainingService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
		{
			MagnumApiUrl = configuration.GetValue<string>("ServiceUrls:MagnumApiUrl");
		}

		public Task<T> CreateAsync<T>(TrainingSession training)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Url = MagnumApiUrl + "/api/Trainings",
				Data = training

			});
		}

		public Task<T> DeleteAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = MagnumApiUrl + "/api/Trainings",

			});
		}

		public Task<T> GetAllAsync<T>()
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = MagnumApiUrl + "/api/Trainings",

			});
		}

		public Task<T> GetAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = MagnumApiUrl + "/api/Trainings" + id,
				
			});
		}

		public Task<T> UpdateAsync<T>(TrainingSession training)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				Url = MagnumApiUrl + "/api/Trainings" + training.Id,

			});
		}
	}
}
