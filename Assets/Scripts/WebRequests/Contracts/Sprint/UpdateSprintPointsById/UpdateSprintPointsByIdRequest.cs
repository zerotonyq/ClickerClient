using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Sprint.UpdateSprintPointsById
{
    public class UpdateSprintPointsByIdRequest : WebRequestDto<UpdateSprintPointsByIdResponse>
    {
        public int UserId { get; set; }
        public int Points { get; set; }
    }
}