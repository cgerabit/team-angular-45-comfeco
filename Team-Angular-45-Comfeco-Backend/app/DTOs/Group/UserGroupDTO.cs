using System.Collections.Generic;

namespace BackendComfeco.DTOs.Group
{
    public class UserGroupDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<GroupMember> Members { get; set; }
        public string GroupImage { get; set; }
    }
}
