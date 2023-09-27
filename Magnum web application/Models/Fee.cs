using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Magnum_web_application.Models
{
	public class Fee
	{
		public int Id { get; set; }
		public DateTime DatePaid { get; set; }

		//navigation properties

		public Member Member{ get; set; }
		public int MemberId { get; set; }
	}
}
