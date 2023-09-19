using Magnum_Web_App.Models;
using Magnum_Web_App.Models.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magnum_Web_App.Models
{
	public class TrainingSession
	{
		public int Id { get; set; }
		public bool isTraining { get; set; }
		public int TotalSessions { get; set; }
		public int MonthlySessions { get; set; } 
		public DateTime SessionDate { get; set; }

		public MemberDTO Member { get; set; }
		[ForeignKey(nameof(Member))]
		public int MemberId { get; set; }

		public void AddSession()
		{
			TotalSessions++;
			MonthlySessions++;
			SessionDate = DateTime.Now;
		}

		public bool CheckIsTraining()
		{
			var month = SessionDate.AddDays(30);
			var timeSinceLastSession = SessionDate.Subtract(month);
			if (MonthlySessions >= 3 && (timeSinceLastSession != null))
			{
				return isTraining = true;
			}
			return isTraining = false;
		}
	}

	
}


