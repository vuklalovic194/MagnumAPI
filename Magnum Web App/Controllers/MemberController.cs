using Magnum_Web_App.Models;
using Magnum_Web_App.Models.DTO;
using Magnum_Web_App.Models.ViewModel;
using Magnum_Web_App.Services;
using Magnum_Web_App.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Magnum_Web_App.Controllers
{
	public class MemberController : Controller
	{
		private readonly IMemberService memberService;
		private readonly ITrainingService trainingService;
		private readonly IFeeService feeService;

		public MemberController(IMemberService memberService, ITrainingService trainingService, IFeeService feeService)
        {
			this.memberService = memberService;
			this.trainingService = trainingService;
			this.feeService = feeService;
		}

		[HttpGet]
        public async Task<IActionResult> IndexMember()
		{
			List<MemberDTO> list = new();
			List<TrainingSession> trainingList = new();
			List<Fee> feeList = new();

			var feeResponse = await feeService.GetAllAsync<ApiResponse>();
			if (feeResponse != null && feeResponse.IsSuccess)
			{
				feeList = JsonConvert.DeserializeObject<List<Fee>>(Convert.ToString(feeResponse.Response));
			}

			var trainingResponse = await trainingService.GetAllAsync<ApiResponse>();
			if (trainingResponse != null && trainingResponse.IsSuccess)
			{
				trainingList = JsonConvert.DeserializeObject<List<TrainingSession>>(Convert.ToString(trainingResponse.Response));
			}

			var response = await memberService.GetAllAsync<ApiResponse>();
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<MemberDTO>>(Convert.ToString(response.Response));
			}

			//List<Fee> feeList = new List<Fee>();
			//feeList.Add(fee);

			//List<TrainingSession> trainingSessions = new List<TrainingSession>();
			//trainingSessions.Add(training);

			

			MemberViewModel model = new MemberViewModel()
			{
				Fees = feeList,
				TrainingSessions = trainingList,
				Members = list
			};

			return View(model);
		}

		//[HttpPost]
		//public async Task <IActionResult> IndexMember()
		//{
		//	List<TrainingSession> trainingList = new();
		//	List<Fee> feeList = new();

		//	var feeResponse = await feeService.GetAllAsync<ApiResponse>();
		//	if (feeResponse != null && feeResponse.IsSuccess)
		//	{
		//		feeList = JsonConvert.DeserializeObject<List<Fee>>(Convert.ToString(feeResponse.Response));
		//	}

		//	var trainingResponse = await trainingService.GetAllAsync<ApiResponse>();
		//	if (trainingResponse != null && trainingResponse.IsSuccess)
		//	{
		//		trainingList = JsonConvert.DeserializeObject<List<TrainingSession>>(Convert.ToString(trainingResponse.Response));
		//	}

		//	try
		//	{
		//		if (trainingList != null && feeList != null)
		//		{
		//			foreach (var tra in trainingList)
		//			{
		//				tra.AddSession();
		//				if (tra.CheckIsTraining() == true)
		//				{
		//					foreach (var fee in feeList)
		//					{
		//						fee.CheckIfPaid();
		//						await feeService.UpdateAsync<ApiResponse>(fee);
		//					}
		//				}
		//				var traResponse = await trainingService.UpdateAsync<ApiResponse>(tra);
		//				if (traResponse != null && traResponse.IsSuccess)
		//				{
		//					return RedirectToAction(nameof(IndexMember));
		//				}
		//			}
		//		}
		//	}
		//	catch (Exception)
		//	{
		//	}

		//	return RedirectToAction("IndexMember");
		//}

		public async Task<IActionResult> CreateMember()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> CreateMember(MemberDTO dto)
		{
			var response = await memberService.CreateAsync<ApiResponse>(dto);

			if (response != null && response.IsSuccess)
			{
				return RedirectToAction(nameof(IndexMember));
			}

			return View(dto);
		}

		[HttpPost]
		public async Task<IActionResult> ManageSession(int id)
		{
			TrainingSession training = new();
			Fee fee = new Fee();
			var feeResponse = await feeService.GetAsync<ApiResponse>(id);
			var response = await trainingService.GetAsync<ApiResponse>(id);

			if(feeResponse != null && feeResponse.IsSuccess)
			{
				fee = JsonConvert.DeserializeObject<Fee>(Convert.ToString(response.Response));
			}

			if(response !=null && response.IsSuccess)
			{
				training = JsonConvert.DeserializeObject<TrainingSession>(Convert.ToString(response.Response));
			}
			
			training.AddSession();
			if(training.CheckIsTraining() == true)
			{
				fee.CheckIfPaid();
			}

			

			return RedirectToAction(nameof(IndexMember));
		}
	}
}
