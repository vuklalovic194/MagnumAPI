using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magnum_web_application.Models
{
	public class Member
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[MaxLength(100)]
		public string Address { get; set; }
		public string ImageUrl { get; set; }
		public int PhoneNumber { get; set; }
		[Required]
		public int Age { get; set; }
		public bool VIP { get; set; } = false;
		public string Rank { get; set; }

		public DateTime DateUpdated { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;

		//navigation properties

		public ICollection<Fee> Fee { get; set; }
		public ICollection<TrainingSession> TrainingSession { get; set; }

	}
}
