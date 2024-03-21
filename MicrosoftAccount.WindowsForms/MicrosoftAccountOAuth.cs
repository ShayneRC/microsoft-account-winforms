using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace MicrosoftAccount.WindowsForms
{
    public static class MicrosoftAccountOAuth
    {
        public static async Task<string> LoginOneTimeAuthorizationAsync(string clientId, string[] scopes, IWin32Window owner = null) => await FormMicrosoftAccountAuth.GetAuthenticationToken(clientId, scopes, OAuthFlow.ImplicitGrant, owner);

        public static async Task<AppTokenResult> LoginAuthorizationCodeFlowAsync(string clientId, string[] scopes, IWin32Window owner = null)
        {
            var authorizationCode = await FormMicrosoftAccountAuth.GetAuthenticationToken(clientId, scopes, OAuthFlow.AuthorizationCodeGrant, owner);
            if (string.IsNullOrEmpty(authorizationCode))
                return null;

            var tokens = await RedeemAuthorizationCodeAsync(clientId, FormMicrosoftAccountAuth.OAuthDesktopEndPoint, authorizationCode);

            // This needs the user.read scope to work.
            tokens.Email = await GetEmailAsync(tokens.AccessToken);

            return tokens;
        }

        public static async Task<AppTokenResult> RedeemRefreshTokenAsync(string clientId, string refreshToken)
        {
            var queryBuilder = new QueryStringBuilder();
            queryBuilder.Add("client_id", clientId);
            queryBuilder.Add("redirect_uri", FormMicrosoftAccountAuth.OAuthDesktopEndPoint);
            queryBuilder.Add("refresh_token", refreshToken);
            queryBuilder.Add("grant_type", "refresh_token");

            return await PostToTokenEndPoint(queryBuilder);
        }

        private static async Task<AppTokenResult> PostToTokenEndPoint(QueryStringBuilder queryBuilder)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = WebRequest.CreateHttp(FormMicrosoftAccountAuth.OAuthMSATokenService);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (StreamWriter requestWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                await requestWriter.WriteAsync(queryBuilder.ToString());
                await requestWriter.FlushAsync();
            }

            HttpWebResponse httpResponse;
            try
            {
                var response = await request.GetResponseAsync();
                httpResponse = response as HttpWebResponse;
            }
            catch (WebException webex)
            {
                httpResponse = webex.Response as HttpWebResponse;
            }
            catch (Exception ex)
            {
                return new AppTokenResult() { Error = ex.Message };
            }

            if (httpResponse == null)
                return new AppTokenResult() { Error = "httpResponse was null" };

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var appTokenResult = new AppTokenResult() { Error = $"{httpResponse.StatusCode}: {httpResponse.StatusDescription}" };
                httpResponse.Dispose();
                return appTokenResult;
            }

            try
            {
                using (var responseBodyStreamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseBody = await responseBodyStreamReader.ReadToEndAsync();
                    var tokenResult = JsonConvert.DeserializeObject<AppTokenResult>(responseBody);

                    httpResponse.Dispose();
                    return tokenResult;
                }
            }
            catch (Exception ex)
            {
                return new AppTokenResult() { Error = $"Error reading response: {ex.Message}" };
            }
        }

        private static async Task<AppTokenResult> RedeemAuthorizationCodeAsync(string clientId, string redirectUrl, string authCode)
        {
            QueryStringBuilder queryBuilder = new QueryStringBuilder();
            queryBuilder.Add("client_id", clientId);
            queryBuilder.Add("redirect_uri", redirectUrl);
            queryBuilder.Add("code", authCode);
            queryBuilder.Add("grant_type", "authorization_code");

            return await PostToTokenEndPoint(queryBuilder);
        }

        private static async Task<string> GetEmailAsync(string accessToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = WebRequest.CreateHttp(FormMicrosoftAccountAuth.OAuthGraphMe);
            request.Method = "GET";
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            HttpWebResponse httpResponse;
            try
            {
                var response = await request.GetResponseAsync();
                httpResponse = response as HttpWebResponse;
            }
            catch (WebException webex)
            {
                httpResponse = webex.Response as HttpWebResponse;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            if (httpResponse == null)
                return "httpResponse was null";

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var responseMsg = $"{httpResponse.StatusCode}: {httpResponse.StatusDescription}";
                httpResponse.Dispose();
                return responseMsg;
            }

            try
            {
                using (var responseBodyStreamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseBody = await responseBodyStreamReader.ReadToEndAsync();
                    var userDetails = JsonConvert.DeserializeObject<UserDetails>(responseBody);

                    httpResponse.Dispose();
                    return userDetails.UserPrincipalName;
                }
            }
            catch (Exception ex)
            {
                return $"Error reading response: {ex.Message}";
            }
        }
    }
}
