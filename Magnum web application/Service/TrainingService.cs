using AutoMapper.Execution;
using Magnum_web_application.Models;
using Magnum_web_application.Repository;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.IServices;
using System.Net;

namespace Magnum_web_application.Service
{
	public class TrainingService : ITrainingService
	{
		public ApiResponse apiResponse;
		private readonly ITrainingSessionRepository trainingSessionRepository;
		private readonly IActiveMemberRepository activeMemberRepository;
		private readonly IUnpaidMonthRepository unpaidMonthRepository;
		private readonly IFeeRepository feeRepository;
		private readonly IMemberRepository memberRepository;

		public TrainingService(
			ITrainingSessionRepository trainingSessionRepository,
			IActiveMemberRepository activeMemberRepository,
			IUnpaidMonthRepository unpaidMonthRepository,
			IFeeRepository feeRepository,
			IMemberRepository memberRepository)
		{
			this.apiResponse = new();
			this.trainingSessionRepository = trainingSessionRepository;
			this.activeMemberRepository = activeMemberRepository;
			this.unpaidMonthRepository = unpaidMonthRepository;
			this.feeRepository = feeRepository;
			this.memberRepository = memberRepository;
		}

		public async Task<ApiResponse> CreateSessionAsync(List<int> memberIds)
		{
			foreach (var member in memberIds)
			{

				if (await memberRepository.GetByIdAsync(u => u.Id == member) == null)
				{
					return apiResponse.BadRequest(member);
				}

				List<ActiveMember> activeMemberByMonths = await activeMemberRepository.GetAllAsync(u => u.MemberId == member);
				List<TrainingSession> trainingSessions = await trainingSessionRepository.GetAllAsync(u => u.MemberId == member);
				List<UnpaidMonth> unpaidMonths = await unpaidMonthRepository.GetAllAsync(tracked: false);
				List<Fee> fees = await feeRepository.GetAllAsync(u => u.MemberId == member);
				List<ActiveMember> listToAdd = new List<ActiveMember>();

				//Create Active Member
				await CreateActiveMemberAsync(member, activeMemberByMonths, trainingSessions, listToAdd);

				//Create Unpaid Month
				await CreateUnpaidMonthAsync(member, activeMemberByMonths, fees, unpaidMonths);

				TrainingSession trainingSession = new()
				{
					MemberId = member,
					SessionDate = DateTime.UtcNow
				};

				await trainingSessionRepository.CreateAsync(trainingSession);
			}

			apiResponse.StatusCode = HttpStatusCode.Created;
			return apiResponse;
		}

		public async Task<ApiResponse> DeleteSessionAsync(DateTime date)
		{
			TrainingSession trainingSession = await trainingSessionRepository.GetByIdAsync(u => u.SessionDate == date);
			if (trainingSession == null)
			{
				return apiResponse.NotFound(trainingSession);
			}

			await trainingSessionRepository.DeleteAsync(trainingSession);
			await trainingSessionRepository.SaveAsync();

			apiResponse.StatusCode = HttpStatusCode.NoContent;
			return apiResponse;
		}

		public async Task<ApiResponse> GetSessionHistoryAsync(int memberId)
		{
			List<TrainingSession> trainingSessions = await trainingSessionRepository.GetAllAsync(u => u.MemberId == memberId);

			if (trainingSessions.Count != 0)
			{
				return apiResponse.Get(trainingSessions);
			}
			return apiResponse.NotFound(trainingSessions);
		}

		public async Task<ApiResponse> GetSessionsByMemberIdAsync(int memberId, int month = 0)
		{
			List<TrainingSession> trainingSession = await trainingSessionRepository.GetAllAsync(u => u.MemberId == memberId);
			if (trainingSession.Count != 0)
			{
				if (month != 0)
				{
					trainingSession = await trainingSessionRepository.GetAllAsync(u => u.SessionDate.Month == month && u.MemberId == memberId);

					return apiResponse.Get(trainingSession.Count);
				}

				return apiResponse.Get(trainingSession.Count);
			}

			return apiResponse.NotFound(trainingSession);
		}

		public async Task<bool> CreateActiveMemberAsync(
			int id,
			List<ActiveMember> activeMemberByMonths,
			List<TrainingSession> trainingSessions,
			List<ActiveMember> listToAdd)
		{
			try
			{
				bool isActive;
				var distinctMonths = trainingSessions
					.Select(session => new DateTime(session.SessionDate.Year, session.SessionDate.Month, 1))
					.Distinct();

				foreach (var month in distinctMonths)
				{
					isActive = trainingSessions
						.Count(session => new DateTime(session.SessionDate.Year, session.SessionDate.Month, 1) == month) >= 2;

					if (isActive)
					{
						ActiveMember newMember = new()
						{
							MemberId = id,
							Month = month,
						};

						int memberIdToCheck = id;
						DateTime monthToCheck = month;

						bool containsMember = activeMemberByMonths.Any(member =>
							member.MemberId == memberIdToCheck && member.Month == monthToCheck);

						if (!containsMember)
						{
							listToAdd.Add(newMember);

							await activeMemberRepository.CreateAsync(newMember);
						}
					}
				}
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<bool> CreateUnpaidMonthAsync(
			int member,
			List<ActiveMember> activeMemberByMonths,
			List<Fee> fees,
			List<UnpaidMonth> unpaidMonths)
		{
			List<DateTime> listOfMonth = new List<DateTime>();
			List<DateTime> listOfFees = new List<DateTime>();

			try
			{
				for (int i = 0; i < activeMemberByMonths.Count; i++)
				{
					listOfMonth.Add(activeMemberByMonths[i].Month);
				}
				for (int j = 0; j < fees.Count; j++)
				{
					listOfFees.Add(fees[j].DatePaid);
				}

				var unmatchedMonths = listOfMonth.Except(listOfFees).ToList();

				foreach (var date in unmatchedMonths)
				{
					if (!unpaidMonths.Any(unpaid => unpaid.Month == date && unpaid.MemberId == member))
					{
						UnpaidMonth unpaidMonth = new()
						{
							Month = date,
							MemberId = member,
						};

						await unpaidMonthRepository.CreateAsync(unpaidMonth);
					}
				}
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
