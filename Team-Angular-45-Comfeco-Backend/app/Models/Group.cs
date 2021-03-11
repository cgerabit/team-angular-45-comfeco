using System.Collections.Generic;

namespace BackendComfeco.Models
{
    public class Group :IIdHelper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TechnologyId { get; set; }
        public string Description { get; set; }
        public Technology Technology { get; set; }

        public string GroupImage { get; set; }
        public List<ApplicationUser> Users { get; set; }

    }
}
