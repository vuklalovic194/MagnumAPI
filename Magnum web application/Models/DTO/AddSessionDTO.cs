namespace Magnum_web_application.Models.DTO
{
	public class AddSessionDTO
	{
		public int Debt { get; set; }
		public DateTime DatePaid { get; set; }
		public bool IsPaid { get; set; } = false;

		public bool isTraining { get; set; }
		public int MonthlySessions { get; set; }
		public int TotalSessions { get; set; }
		public DateTime SessionDate { get; set; }
		public bool isActive { get; set; }
		
		public bool CheckIfPaid()
		{
			if (IsPaid)
			{
				DatePaid = DateTime.Now;
				if (Debt > 0)
					Debt -= 4000;
				else
					Debt = 0;
				return true;
			}
			else
			{
				if (DatePaid.AddMinutes(1) != DatePaid)
				{
					IsPaid = false;
					Debt += 4000;
					DatePaid = DateTime.Now;
				}
				return false;
			}
		}

		public void AddSession()
		{
			if (isActive)
			{
				SessionDate = DateTime.Now;
				TotalSessions++;
				MonthlySessions++;
			}
		}

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

