using AutoMapper.Execution;

namespace Magnum_web_application.Models.DTO
{
	public class FeeDTO
	{
		public int MemberId { get; set; }
		public DateTime DatePaid { get; set; }

		public void CreateFeeDTO(int memberId)
		{
			FeeDTO feeDTO = new()
			{
				MemberId = memberId,
				DatePaid = DateTime.Now,
			};
		}
	}
}
