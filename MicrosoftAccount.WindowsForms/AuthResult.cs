using System;

namespace MicrosoftAccount.WindowsForms
{
    public class AuthResult
    {
        public OAuthFlow AuthFlow { get; private set; }
        public string AuthorizeCode { get; private set; }
        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
        public string AccessToken { get; private set; }
        public string AuthenticationToken { get; private set; }
        public string TokenType { get; private set; }
        public TimeSpan AccessTokenExpiresIn { get; private set; }
        public string[] Scopes { get; private set; }
        public string UserId { get; private set; }

        public AuthResult(Uri resultUri, OAuthFlow flow)
        {
            AuthFlow = flow;

            string[] queryParams;
            switch (flow)
            {
                case OAuthFlow.ImplicitGrant:
                    int accessTokenIndex = resultUri.AbsoluteUri.IndexOf("#access_token");
                    queryParams = accessTokenIndex > 0
                        ? resultUri.AbsoluteUri.Substring(accessTokenIndex + 1).Split('&')
                        : resultUri.Query.TrimStart('?').Split('&');
                    break;
                case OAuthFlow.AuthorizationCodeGrant:
                    queryParams = resultUri.Query.TrimStart('?').Split('&');
                    break;
                default:
                    throw new NotSupportedException("flow value not supported");
            }

            foreach (string param in queryParams)
            {
                string[] kvp = param.Split('=');
                switch (kvp[0])
                {
                    case "code":
                        AuthorizeCode = kvp[1];
                        break;
                    case "access_token":
                        AccessToken = kvp[1];
                        break;
                    case "authorization_token":
                    case "authentication_token":
                        AuthenticationToken = kvp[1];
                        break;
                    case "error":
                        ErrorCode = kvp[1];
                        break;
                    case "error_description":
                        ErrorDescription = Uri.UnescapeDataString(kvp[1]);
                        break;
                    case "token_type":
                        TokenType = kvp[1];
                        break;
                    case "expires_in":
                        AccessTokenExpiresIn = new TimeSpan(0, 0, int.Parse(kvp[1]));
                        break;
                    case "scope":
                        Scopes = kvp[1].Split(new string[] { "%20" }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "user_id":
                        UserId = kvp[1];
                        break;
                }
            }
        }
    }
}
