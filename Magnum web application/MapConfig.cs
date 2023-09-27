using AutoMapper;
using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;

namespace Magnum_web_application
{
	public class MapConfig : Profile
	{
        public MapConfig()
        {
            CreateMap<Member, MemberDTO>().ReverseMap();
            CreateMap<Fee, FeeDTO>().ReverseMap();
		}
    }
}
