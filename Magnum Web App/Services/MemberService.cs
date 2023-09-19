using Magnum_Web_App.Models;
using Magnum_Web_App.Models.DTO;
using Magnum_Web_App.Services.IServices;
using MagnumApp_Utility;

namespace Magnum_Web_App.Services
{
	public class MemberService : BaseService, IMemberService
	{
		private readonly IConfiguration configuration;
		private readonly IHttpClientFactory httpClientFactory;
		private string MagnumApiUrl;

		public MemberService( IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
			this.httpClientFactory = httpClientFactory;
			MagnumApiUrl = configuration.GetValue<string>("ServiceUrls:MagnumApiUrl");
		}

        public Task<T> CreateAsync<T>(MemberDTO memberDTO)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Url = MagnumApiUrl + "/api/Members",
				Data = memberDTO
				
			});
		}

		public Task<T> DeleteAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = MagnumApiUrl + "/api/Members" + id,
			});
		}

		public Task<T> GetAllAsync<T>()
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = MagnumApiUrl + "/api/Members",

			});
		}

		public Task<T> GetAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = MagnumApiUrl + "/api/Members" + id
			});
		}

		public Task<T> UpdateAsync<T>(MemberDTO memberDTO)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				Url = MagnumApiUrl + "/api/Members" + memberDTO.Id,
				Data = memberDTO
			});
		}
	}
}
