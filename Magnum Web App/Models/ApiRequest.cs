using Microsoft.AspNetCore.Mvc;
using static MagnumApp_Utility.SD;

namespace Magnum_Web_App.Models
{
	public class ApiRequest
	{
		public ApiType ApiType { get; set; } = ApiType.GET;
		public string Url { get; set; }
		public object Data { get; set; }
	}
}
