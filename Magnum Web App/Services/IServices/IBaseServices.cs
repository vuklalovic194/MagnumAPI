using Magnum_Web_App.Models;

namespace Magnum_Web_App.Services.IServices
{
	public interface IBaseServices
	{
		public ApiResponse responseModel { get; set; }

		Task <T> SendAsync<T> (ApiRequest apiRequest);
	}
}
