export const environment = {
  production: true,
  baseUrl: "https://comfeco.cristiangerani.com/api",
  tokenFieldName :"access_token",
  tokenExpirationFieldName:"token_expiration",
  bearertokenResponseTypeName : "Bearer",
  externalLoginEndpoint:"https://team45.comfeco.cristiangerani.com/auth/external-signin-callback",
  persistLoginEndpoint:"https://team45.comfeco.cristiangerani.com/auth/persistent-signin-callback",
  redirectionLoginSecurityKeyFieldName:"securitykey",
  isPersistentKeyFieldName:"isPersistent",
  externalLoginTokenPurposeName:"External-Auth-Code",
  persistentLoginTokenPurposeName:"Persistent-Auth-Code",

  claimTypes:
  {
    username:"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
    userId:"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
    email:"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
  }
};


