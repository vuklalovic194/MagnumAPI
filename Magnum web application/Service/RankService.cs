using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.IServices;

namespace Magnum_web_application.Service
{
	public class RankService : IRankService
	{
		public ApiResponse apiResponse;
		private readonly IRankRepository rankRepository;

		public RankService(IRankRepository rankRepository)
		{
			this.rankRepository = rankRepository;
			this.apiResponse = new();
		}

		public async Task<ApiResponse> CreateRankAsync(RankDTO rankDTO, int memberId)
		{
			List<Rank> ranks = await rankRepository.GetAllAsync(r => r.MemberId == memberId);
			
			foreach (Rank rank in ranks)
			{
				if (rank.MemberId == memberId && rank.SkillRank == rankDTO.SkillRank)
				{
					return apiResponse.BadRequest();
				}
			}
			
			Rank model = new()
			{
				SkillRank = rankDTO.SelectRank(rankDTO.SkillRank),
				MemberId = memberId,
				Promotion = DateTime.UtcNow
			};

			await rankRepository.CreateAsync(model);
			return apiResponse.Create(rankDTO);
		}

		public async Task<ApiResponse> GetAllRanksAsync(int memberId)
		{
			List<Rank> listOfRanks = await rankRepository.GetAllAsync(r => r.MemberId == memberId);
			return apiResponse.Get(listOfRanks);
		}

		public async Task<ApiResponse> GetRankAsync(int memberId)
		{
			Rank rank = await rankRepository.GetByIdAsync(r => r.MemberId == memberId);
			return apiResponse.Get(rank);
		}
	}
}
