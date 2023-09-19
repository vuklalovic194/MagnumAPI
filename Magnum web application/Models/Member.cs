using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magnum_web_application.Models
{
	public class Member
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[MaxLength(100)]
		public string Address { get; set; }
		public string ImageUrl { get; set; }
		public int PhoneNumber { get; set; }
		[Required]
		public int Age { get; set; }
		public string Rank { get; set; }

		public DateTime DateUpdated { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;

		public int Debt { get; set; }
		public DateTime DatePaid { get; set; }
		public bool IsPaid { get; set; } = false;
		public bool VIP { get; set; } = false;

		public bool CheckIfPaid()
		{
			if (VIP == false)
			{
				if (IsPaid)
				{
					IsPaid = true;
					DatePaid = DateTime.Now;
					if (Debt > 0)
						Debt -= 4000;
					else
						Debt = 0;
					return true;
				}
				else
				{
					if (DatePaid.AddDays(30) == DatePaid)
					{
						IsPaid = false;
						Debt += 4000;
					}
					return false;
				}
			}
			else
			{
				return true;
			}
			
		}

		public bool isTraining { get; set; }
		public int MonthlySessions { get; set; } 
		public int TotalSessions { get; set; }
		public DateTime SessionDate { get; set; }
		
		public bool CheckIsTraining()
		{
			if (SessionDate.AddDays(30) != SessionDate && 
				SessionDate.AddDays(30) != SessionDate || MonthlySessions >= 3)
			{
				return true;
			}
			MonthlySessions = 0;
			return false;
		}
	}
}
