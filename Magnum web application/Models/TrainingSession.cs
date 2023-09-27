using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository.IRepository;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Magnum_web_application.Models
{
	public class TrainingSession
	{
		public int Id { get; set; }
		public DateTime SessionDate { get; set; }

		[JsonIgnore]
		public Member Member { get; set; }
		public int MemberId { get; set; }
	}
}
