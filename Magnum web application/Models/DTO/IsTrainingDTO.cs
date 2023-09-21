using AutoMapper.Execution;

namespace Magnum_web_application.Models.DTO
{
	public class IsTrainingDTO
	{
		public bool IsTraining { get; set; } = false;

		public void isTraining(TrainingSession trainingSession)
		{
			trainingSession.isTraining = IsTraining;
		}
	}
}
