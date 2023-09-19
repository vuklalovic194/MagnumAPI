using Magnum_Web_App.Models;
using Magnum_Web_App.Services.IServices;
using MagnumApp_Utility;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Magnum_Web_App.Services
{
	public class BaseService : IBaseServices
	{
		private readonly IHttpClientFactory httpClient;
		protected string magnumApiUrl;

		public ApiResponse responseModel { get; set; }

		public BaseService(IHttpClientFactory httpClient)
		{
			responseModel = new();
			this.httpClient = httpClient;
		}

		public async Task<T> SendAsync<T>(ApiRequest apiRequest)
		{
			try
			{
				var client = httpClient.CreateClient("MagnumApi");
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
				httpRequestMessage.Headers.Add("Accept", "application/json");
				httpRequestMessage.RequestUri = new Uri(apiRequest.Url);
				if (apiRequest.Data != null)
				{
					httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
				}
				switch (apiRequest.ApiType)
				{
					case SD.ApiType.POST:
						httpRequestMessage.Method = HttpMethod.Post;
						break;
					case SD.ApiType.PUT:
						httpRequestMessage.Method = HttpMethod.Put;
						break;
					case SD.ApiType.DELETE:
						httpRequestMessage.Method = HttpMethod.Delete;
						break;
					default:
						httpRequestMessage.Method = HttpMethod.Get;
						break;
				}

				HttpResponseMessage apiResponse = null;

				apiResponse = await client.SendAsync(httpRequestMessage);

				var apiContent = await apiResponse.Content.ReadAsStringAsync();
				var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);

				return APIResponse;
			}
			catch (Exception e)
			{
				var dto = new ApiResponse
				{
					ErrorMessage = e.Message,
					IsSuccess = false,
				};
				var res = JsonConvert.SerializeObject(dto);
				var APIResponse = JsonConvert.DeserializeObject<T>(res);
				return APIResponse;
			}

		}
	}
}
