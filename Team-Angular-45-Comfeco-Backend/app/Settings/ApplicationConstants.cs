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

        public const string LoginFrontendDefaultEndpoint = "http://localhost:4200/auth/login";

        public const string ConfirmEmailFrontendDefaultEndpoint = "http://localhost:4200/auth/confirm";

        public const string PasswordRecoveryFrontendDefaultEndpoint = "http://localhost:4200/auth/recover";

        public const string CountOfRecordsHeaderName = "CountOfRecords"; 

        public static class ImageContainerNames
        {
            public const string SponsorContainer = "Sponsor_Icons";
        }
    }
}
