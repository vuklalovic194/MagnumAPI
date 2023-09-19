using Magnum_Web_App.Models.DTO;

namespace Magnum_Web_App.Models.ViewModel
{
	public class MemberViewModel
	{
		public List<MemberDTO> Members {  get; set; }
		public List <TrainingSession> TrainingSessions { get; set; }
		public List <Fee> Fees { get; set; }
	}
}
