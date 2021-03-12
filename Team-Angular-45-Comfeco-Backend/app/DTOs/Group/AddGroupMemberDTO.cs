using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Group
{
    public class AddGroupMemberDTO
    {
        [Required]
        public string UserId { get; set; }
    }
}
