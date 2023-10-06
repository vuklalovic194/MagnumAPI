using System.Net;

namespace Magnum_web_application.Models
{
	public class ApiResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public string ErrorMessage { get; set; }
		public bool IsSuccess { get; set; } = true;
		public object Response { get; set; }

		public ApiResponse BadRequest()
		{
			IsSuccess = false;
			StatusCode = HttpStatusCode.BadRequest;
			ErrorMessage = "Error while creating";

			return this;
		}

		public ApiResponse Create(object model)
		{
			StatusCode = HttpStatusCode.Created;
			Response = model;
			return this;
		}

		public ApiResponse NotFound(object obj)
		{
			IsSuccess = true;
			StatusCode = HttpStatusCode.NotFound;
			ErrorMessage = "Not Found";
			return this;
		}

		public ApiResponse Get(object obj)
		{
			StatusCode = HttpStatusCode.OK;
			Response = obj;
			return this;
		}

		public ApiResponse Update(object obj)
		{
			StatusCode = HttpStatusCode.NoContent;
			Response = obj;
			return this;
		}

		public ApiResponse Unauthorize(Exception e)
		{
			StatusCode = HttpStatusCode.Unauthorized;
			ErrorMessage = e.Message.ToString();
			IsSuccess = false;
			return this;
		}
	}
}
