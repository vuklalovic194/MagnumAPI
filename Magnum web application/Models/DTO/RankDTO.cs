using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Magnum_web_application.Models.DTO
{
	public class RankDTO
	{
		public string SkillRank { get; set; }
		
		public enum Rank
		{
			White,
			Blue,
			Purple,
			Brown,
			Black
		}

		public string SelectRank(string rank)
		{
			switch (rank.ToLower())
			{
				case "blue":
					return SkillRank = Rank.Blue.ToString();
				case "purple":
					return SkillRank = Rank.Purple.ToString();
				case "brown":
					return SkillRank = Rank.Brown.ToString();
				case "black":
					return SkillRank = Rank.Black.ToString();
				default:
					return SkillRank = Rank.White.ToString();
			}
		}
	}
}
