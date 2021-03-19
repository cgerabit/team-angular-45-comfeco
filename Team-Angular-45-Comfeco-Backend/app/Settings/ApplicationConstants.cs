namespace BackendComfeco.Settings
{
    public static class ApplicationConstants
    {
        public const string DevelopmentPolicyName = "Development";

        public const int DefaultExternalRedirectUrlId = 1;

        public const string RegisterTokenResponseName= "Register";

        public const string AuthCodeTokenProviderName = "AuthCode";

        public const string ExternalLoginTokenPurposeName = "External-Auth-Code";

        public const string PersistentLoginTokenPurposeName = "Persistent-Auth-Code";

        public const string KeyHashCookieName = "SecurityKeyHash";

        public const string PersistLoginSchemeName = "PersistLogin";
        public const string PersistLoginCookieName = "PersistLogin";

        public const string LoginFrontendDefaultEndpoint = "https://team45.comfeco.cristiangerani.com/auth/login";

        public const string ConfirmEmailFrontendDefaultEndpoint = "https://team45.comfeco.cristiangerani.com/auth/confirm";

        public const string PasswordRecoveryFrontendDefaultEndpoint = "https://team45.comfeco.cristiangerani.com/auth/recover";

        public const string ProfileFrontendDefaultEndpoint = "https://team45.comfeco.cristiangerani.com/protected/profile";

        public const string CountOfRecordsHeaderName = "CountOfRecords";

        public const string IdCookieName = "IdCookie";
        public const string IdCookieVerifier = "IdCookieVerifier";
        public static class ImageContainerNames
        {
            public const string AreaContainer = "Area_Icons";
            public const string SponsorContainer = "Sponsor_Icons";
            public const string TechnologyContainer = "Technology_Icons";
            public const string ProfilePicturesContainer = "User_Profile";
            public const string SocialNetworkIconContainer = "Social_Network";
            public const string GroupImagesContainer = "Group_Img";
            public const string BadgePicturesContainer = "Badge_Icons";
            public const string EventImagesContainer = "Event_Icons";
        }

        public static class Roles
        {
            public const string ContentCreatorRoleId = "6a8af04b-0405-4cd2-bc20-d59433235153";
            public const string ContentCreatorRoleName = "ContentCreator";
        }
    }
}
