using Magnum_web_application.Models;
using Magnum_web_application.Models.DTO;
using Magnum_web_application.Repository.IRepository;
using Magnum_web_application.Service.IServices;
using System.Net;

namespace Magnum_web_application.Service
{
    public class MemberService : IMemberService
    {
        public ApiResponse apiResponse;
        private readonly IMemberRepository repository;

        public MemberService(IMemberRepository repository)
        {
            this.apiResponse = new();
            this.repository = repository;
        }

        public async Task<ApiResponse> CreateMemberIfValidAsync(MemberDTO memberDTO)
        {
            try
            {
                if (await repository.GetByIdAsync(u => u.Name == memberDTO.Name) != null)
                {
                    apiResponse.BadRequest();
                    apiResponse.ErrorMessage = "Member with same name already exists";
                    return apiResponse;
                }

                Member model = new();
                memberDTO.mapMember(memberDTO, model);

                await repository.CreateAsync(model);
                await repository.SaveAsync();

                return apiResponse.Create(model);
            }
            catch (Exception e)
            {
                return apiResponse.Unauthorize(e);
            }
        }

        public async Task<ApiResponse> DeleteMemberAsync(int memberId)
        {
            try
            {
                Member member = await repository.GetByIdAsync(u => u.Id == memberId);
                if (member == null)
                {
                    return apiResponse.NotFound(member);
                }

                await repository.DeleteAsync(member);
                await repository.SaveAsync();

                apiResponse.StatusCode = HttpStatusCode.NoContent;
                return apiResponse;
            }
            catch (Exception e)
            {
                return apiResponse.Unauthorize(e);
            }
        }

        public async Task<ApiResponse> GetMemberByIdAsync(int memberId)
        {
            try
            {
                Member member = await repository.GetByIdAsync(u => u.Id == memberId);
                if (member == null)
                {
                    return apiResponse.NotFound(member);
                }

                return apiResponse.Get(member);
            }
            catch (Exception e)
            {
                return apiResponse.Unauthorize(e);
            }
        }

        public async Task<ApiResponse> GetMembersAsync()
        {
            try
            {
                List<Member> memberList = await repository.GetAllAsync();

                if (memberList.Count <= 0)
                {
                    apiResponse.NotFound(memberList);
                }

                return apiResponse.Get(memberList);
            }
            catch (Exception e)
            {
                return apiResponse.Unauthorize(e);
            }
        }

        public async Task<ApiResponse> UpdateMemberAsync(MemberDTO memberDTO, int memberId)
        {
            try
            {
                Member member = await repository.GetByIdAsync(u => u.Id == memberId);
                if (member == null)
                {
                    return apiResponse.NotFound(member);
                }

                memberDTO.mapMember(memberDTO, member);
                await repository.Update(member);
                await repository.SaveAsync();

                return apiResponse.Update(member);
            }
            catch (Exception e)
            {
                return apiResponse.Unauthorize(e);
            }
        }
    }
}
