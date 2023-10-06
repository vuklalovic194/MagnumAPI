using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;

namespace Magnum_web_application.Repository
{
	public class RankRepository : Repository<Rank>, IRankRepository
	{
        private readonly ApplicationDbContext context;
       
        public RankRepository(ApplicationDbContext context) : base(context) 
        {
            this.context = context;
        }
    }
}
