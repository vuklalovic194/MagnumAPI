using System.Text.Json.Serialization;

namespace Magnum_web_application.Models
{
	public class Rank
	{
		public int Id { get; set; }
		public DateTime Promotion { get; set; }
		public string SkillRank { get; set; }

		[JsonIgnore]
		public Member Member { get; set; }
		public int MemberId { get; set; }
	}
}
