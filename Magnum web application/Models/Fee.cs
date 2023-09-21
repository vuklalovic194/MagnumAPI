﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Magnum_web_application.Models
{
	public class Fee
	{
		public int Id { get; set; }
		public int Debt { get; set; }
		public DateTime DatePaid { get; set; }
		public bool IsPaid { get; set; }

		public Member Member{ get; set; }
		[ForeignKey(nameof(Member))]
		public int MemberId { get; set; }

		public void CheckIfPaid()
		{
			if (IsPaid)
			{
				Debt -= 4000;
				DatePaid = DateTime.Now;
			}
			else
			{
				if (!IsPaid)
				{
					DatePaid.AddDays(30);
					Debt += 4000;
				}
			}
		}
	}
}
