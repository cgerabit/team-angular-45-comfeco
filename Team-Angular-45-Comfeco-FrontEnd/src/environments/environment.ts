// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  baseUrl: "https://comfeco.cristiangerani.com/api",
  tokenFieldName :"access_token",
  tokenExpirationFieldName:"token_expiration",
  bearertokenResponseTypeName : "Bearer",
  externalLoginEndpoint:"http://localhost:4200/auth/external-signin-callback",
  persistLoginEndpoint:"http://localhost:4200/auth/persistent-signin-callback",
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

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
