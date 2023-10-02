using AutoMapper.Execution;
using Magnum_web_application.Controllers;

namespace Magnum_web_application.Models
{
	public class UnpaidMonth
	{
		public int Id { get; set; }
		public DateTime Month { get; set; }

		public Member Member { get; set; }
		public int MemberId { get; set; }
	}
}
