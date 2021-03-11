namespace BackendComfeco.DTOs.Group
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TechnologyId { get; set; }
        public string TechnologyName { get; set; }
        public string GroupImage { get; set; }
    }
}
