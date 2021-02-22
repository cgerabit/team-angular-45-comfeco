namespace BackendComfeco.DTOs.UserRelations
{
    public class ApplicationUserSocialNetworkDTO
    {
        public int SocialNetworkId { get; set; }

        public bool IsPrincipal { get; set; } = false;

        public string Url { get; set; }

    }
}
