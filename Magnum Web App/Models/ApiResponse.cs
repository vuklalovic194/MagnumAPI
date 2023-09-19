using System.Net;

namespace Magnum_Web_App.Models
{
	public class ApiResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public string ErrorMessage { get; set; }
		public bool IsSuccess { get; set; } = true;
		public object Response { get; set; }

	}
}
