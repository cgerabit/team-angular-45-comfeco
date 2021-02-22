using System.Collections.Generic;

namespace BackendComfeco.DTOs.ContentCreators
{
    public class ContentCreatorFilters
    {
        public List<int> TechnologyId { get; set; }
        
        public string UserNameContains { get; set; }
    }
}
