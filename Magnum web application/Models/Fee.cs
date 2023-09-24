using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Magnum_web_application.Models
{
	public class Fee
	{
		public int Id { get; set; }
		public DateTime DatePaid { get; set; }

		//navigation properties
		[JsonIgnore]
		public Member Member{ get; set; }
		public int MemberId { get; set; }

		//public void CheckIfPaid()
		//{
		//	if (IsPaid)
		//	{
		//		Debt -= 4000;
		//		DatePaid = DateTime.Now;
		//	}
		//	else
		//	{
		//		if (!IsPaid)
		//		{
		//			DatePaid.AddMinutes(1);
		//			Debt += 4000;
		//		}
		//	}
		//}
	}
}
