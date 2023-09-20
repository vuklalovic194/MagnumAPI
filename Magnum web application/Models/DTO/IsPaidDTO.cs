using AutoMapper.Execution;

namespace Magnum_web_application.Models.DTO
{
	public class IsPaidDTO
	{
		public bool isPaid { get; set; }
		public void mapPaid(Member member, IsPaidDTO isPaidDTO)
		{
			member.IsPaid = isPaidDTO.isPaid;
		}
	}
}
