namespace Magnum_web_application.Models.DTO
{
	public class MemberDTO
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string ImageUrl { get; set; }
		public int PhoneNumber { get; set; }
		public int Age { get; set; }
		public string Rank { get; set; }
		public bool VIP { get; set; } = false;

		public void mapMember(MemberDTO updateDTO, Member member)
		{
			member.Address = updateDTO.Address;
			member.Name = updateDTO.Name;
			member.ImageUrl = updateDTO.ImageUrl;
			member.PhoneNumber = updateDTO.PhoneNumber;
			member.Age = updateDTO.Age;
			member.VIP = updateDTO.VIP;
		}
	}
}

