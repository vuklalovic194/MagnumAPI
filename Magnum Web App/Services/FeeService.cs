using Magnum_Web_App.Models;
using Magnum_Web_App.Services.IServices;
using MagnumApp_Utility;
using Microsoft.Extensions.Configuration;

namespace Magnum_Web_App.Services
{
	public class FeeService : BaseService, IFeeService
	{
		private string MagnumApiUrl;

		public FeeService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
		{
			MagnumApiUrl = configuration.GetValue<string>("ServiceUrls:MagnumApiUrl");
		}

		public Task<T> CreateAsync<T>(Fee fee)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Url = MagnumApiUrl + "/api/Fees",
				Data = fee

			});
		}

		public Task<T> DeleteAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = MagnumApiUrl + "/api/Fees",

			});
		}

		public Task<T> GetAllAsync<T>()
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = MagnumApiUrl + "/api/Fees",

			});
		}

		public Task<T> GetAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = MagnumApiUrl + "/api/Fees" + id,
				
			});
		}

		public Task<T> UpdateAsync<T>(Fee fee)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				Url = MagnumApiUrl + "/api/Fees" + fee.Id,

			});
		}
	}
}
