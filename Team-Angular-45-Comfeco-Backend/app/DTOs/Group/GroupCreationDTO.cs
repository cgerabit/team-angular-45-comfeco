using Microsoft.AspNetCore.Http;

namespace BackendComfeco.DTOs.Group
{
    public class GroupCreationDTO
    {
        public string Name { get; set; }
        public int TechnologyId { get; set; }
        public string Description { get; set; }
        public IFormFile GroupImage { get; set; }
    }
}
