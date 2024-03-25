# Microsoft Account Windows Forms

This library provides an easy way to generate Microsoft Account access and 
refresh tokens from a Windows Forms application. It has been forked from [here](https://github.com/rgregg/microsoft-account-winforms)
and has been modified to use the Microsoft Identity Platform [docs](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-auth-code-flow)

Before using this library, you need to register your application on the 
[Microsoft Account developer center](https://account.live.com/developers/applications/).
Your application needs to be configured as a Mobile or Desktop Client application on the
API settings page for this library to work.

To retrieve a one-time access token (currently un-tested):
```csharp
using MicrosoftAccount.WindowsForms;

public async void Authenticate()
{
  string accessToken = await MicrosoftAccountOAuth.LoginOneTimeAuthorizationAsync("client_id", 
    new string[] { "https://graph.microsoft.com/user.read", "offline_access" }, true);
}
```

To retrieve a refresh token along with your access token to enable your app to stay
signed-in for longer than the 1 hour access token validitity period:

```csharp
using MicrosoftAccount.WindowsForms;

public async void Authenticate()
{
  bool selectAccount = true;
  var token = await MicrosoftAccountOAuth.LoginAuthorizationCodeFlowAsync("client_id",
    new string[] { "https://graph.microsoft.com/user.read", "offline_access" }, selectAccount);
  
  string accessToken = token.AccessToken;
  string refreshToken = token.RefreshToken;
  string userEmail = token.Email;
  
  // Store these values somewhere useful. Treat them like passwords!
}
```

After the access token has expired, you can redeem the refresh token for a new set of
tokens using `RedeemRefreshTokenAsync()`:

```csharp
using MicrosoftAccount.WindowsForms;

public async void RenewAuthentication()
{
  var token = await MicrosoftAccountOAuth.RedeemRefreshTokenAsync("client_id", "refresh_token");
  
  string accessToken = token.AccessToken;
  string refreshToken = token.RefreshToken;
  
  // Store these values somewhere useful. Treat them like passwords!
}
```
