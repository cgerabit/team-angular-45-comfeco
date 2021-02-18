namespace BackendComfeco.DTOs.Auth
{
    public class LoginWithPersistDTO : BaseRedirectLoginDTO
    {
        public string InternalUrl { get; set; }
    }
}
