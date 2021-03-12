namespace BackendComfeco.DTOs.Group
{
    public class GroupJoinResult
    {
        public bool AlreadyInAgroup { get; set; } = false;
        public bool Success { get; set; }
        public bool AlreadyInThisGroup { get; set; } = false;



    }
}
