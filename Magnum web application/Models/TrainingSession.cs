using System.ComponentModel.DataAnnotations.Schema;

namespace Magnum_web_application.Models
{
	public class TrainingSession
	{
		public int Id { get; set; }
		public bool isTraining { get; set; }
		public int TotalSessions { get; set; }
		public DateTime SessionDate { get; set; }

		public Member Member { get; set; }
		[ForeignKey(nameof(Member))]
		public int MemberId { get; set; }

		public void AddSession()
		{
			SessionDate = DateTime.Now;
			TotalSessions++;
		}
	}
}
