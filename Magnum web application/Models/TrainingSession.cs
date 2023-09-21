using Magnum_web_application.Models.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magnum_web_application.Models
{
	public class TrainingSession
	{
		public int Id { get; set; }
		public bool isTraining { get; set; }
		public int TotalSessions { get; set; }
		public DateTime SessionDate { get; set; }
		public int MonthlySessions { get; set; }

		public Member Member { get; set; }
		[ForeignKey(nameof(Member))]
		public int MemberId { get; set; }

		public void mapForNewSession(IsTrainingDTO isTrainingDTO, int id)
		{
			isTraining = isTrainingDTO.IsTraining;
			MemberId = id;
		}

		public void AddSingleSession()
		{
			if (isTraining)
			{
				SessionDate = DateTime.Now;
				MonthlySessions++;
				TotalSessions++;
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
