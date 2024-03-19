using System;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MicrosoftAccount.WindowsForms
{
    public partial class FormMicrosoftAccountAuth : Form
    {
        public const string OAuthDesktopEndPoint = "https://login.live.com/oauth20_desktop.srf";
        public const string OAuthMSAAuthorizeService = "https://login.live.com/oauth20_authorize.srf";
        public const string OAuthMSATokenService = "https://login.live.com/oauth20_token.srf";

        public string StartUrl { get; private set; }
        public string EndUrl { get; private set; }
        public AuthResult AuthResult { get; private set; }
        public OAuthFlow AuthFlow { get; private set; }

        public FormMicrosoftAccountAuth(string startUrl, string endUrl, OAuthFlow flow = OAuthFlow.AuthorizationCodeGrant)
        {
            InitializeComponent();

            StartUrl = startUrl;
            EndUrl = endUrl;
            AuthFlow = flow;
        }

        public Task<DialogResult> ShowDialogAsync(IWin32Window owner = null)
        {
            TaskCompletionSource<DialogResult> tcs = new TaskCompletionSource<DialogResult>();
            FormClosed += (s, e) => { tcs.SetResult(DialogResult); };

            if (owner == null)
                Show();
            else
                Show(owner);

            return tcs.Task;
        }

        #region Private Methods

        private void FormMicrosoftAccountAuth_Load(object sender, EventArgs e)
        {
            webBrowser.CanGoBackChanged += CanGoBackChanged;
            webBrowser.CanGoForwardChanged += CanGoBackChanged;
            FixUpNavigationButtons();

            webBrowser.Navigated += Navigated;

            Debug.WriteLine($"Navigating to start URL: {StartUrl}");
            webBrowser.Navigate(StartUrl);
        }

        void CanGoBackChanged(object sender, EventArgs e) => FixUpNavigationButtons();

        private void Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Debug.WriteLine($"Navigated to: {webBrowser.Url.AbsoluteUri}");

            Text = webBrowser.DocumentTitle;

            if (webBrowser.Url.AbsoluteUri.StartsWith(EndUrl))
            {
                AuthResult = new AuthResult(webBrowser.Url, AuthFlow);
                CloseWindow();
            }
        }

        private void CloseWindow()
        {
            const int interval = 100;
            var t = new System.Threading.Timer(new System.Threading.TimerCallback((state) =>
            {
                DialogResult = DialogResult.OK;
                BeginInvoke(new MethodInvoker(() => Close()));
            }), null, interval, System.Threading.Timeout.Infinite);
        }

        private void FixUpNavigationButtons()
        {
            toolStripBackButton.Enabled = webBrowser.CanGoBack;
            toolStripForwardButton.Enabled = webBrowser.CanGoForward;
        }

        private void toolStripButton1_Click(object sender, EventArgs e) => webBrowser.GoBack();

        private void toolStripButton2_Click(object sender, EventArgs e) => webBrowser.GoForward();

        #endregion

        #region Static Methods

        private static string GenerateScopeString(string[] scopes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var scope in scopes)
            {
                if (sb.Length > 0)
                    sb.Append(" ");
                sb.Append(scope);
            }
            return sb.ToString();
        }

        private static string BuildUriWithParameters(string baseUri, Dictionary<string, string> queryStringParameters)
        {
            var sb = new StringBuilder();
            sb.Append(baseUri);
            sb.Append("?");
            foreach (var param in queryStringParameters)
            {
                if (sb[sb.Length - 1] != '?')
                    sb.Append("&");
                sb.Append(param.Key);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(param.Value));
            }
            return sb.ToString();
        }

        public static void GenerateUrlsForOAuth(string clientId, string[] scopes, OAuthFlow flow, out string startUrl, out string completeUrl, string redirectUrl = OAuthDesktopEndPoint)
        {
            Dictionary<string, string> urlParam = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "scope", GenerateScopeString(scopes) },
                { "redirect_uri", redirectUrl },
                { "display", "popup" }
            };

            switch (flow)
            {
                case OAuthFlow.ImplicitGrant:
                    urlParam.Add("response_type", "token");
                    break;
                case OAuthFlow.AuthorizationCodeGrant:
                    urlParam.Add("response_type", "code");
                    break;
                default:
                    throw new NotSupportedException("flow value not supported");
            }

            startUrl = BuildUriWithParameters(OAuthMSAAuthorizeService, urlParam);
            completeUrl = redirectUrl;
        }

        public static async Task<string> GetAuthenticationToken(string clientId, string[] scopes, OAuthFlow flow, IWin32Window owner = null)
        {
            GenerateUrlsForOAuth(clientId, scopes, flow, out string startUrl, out string completeUrl);

            FormMicrosoftAccountAuth authForm = new FormMicrosoftAccountAuth(startUrl, completeUrl, flow);
            DialogResult result = await authForm.ShowDialogAsync(owner);

            return DialogResult.OK == result ? OnAuthComplete(authForm.AuthResult) : null;
        }

        private static string OnAuthComplete(AuthResult authResult)
        {
            switch (authResult.AuthFlow)
            {
                case OAuthFlow.ImplicitGrant:
                    return authResult.AccessToken;
                case OAuthFlow.AuthorizationCodeGrant:
                    return authResult.AuthorizeCode;
                default:
                    throw new ArgumentOutOfRangeException("Unsupported AuthFlow value");
            }
        }

        #endregion

        public static async Task<string> GetUserId(string authToken)
        {
            // I believe this method may have been added later and I have not tested it, I have left the URL as the one drive URL for now.
            if (string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException("authToken");

            string requestUrl = $"https://apis.live.net/v5.0/me?access_token={Uri.EscapeUriString(authToken)}";
            HttpWebRequest request = WebRequest.CreateHttp(requestUrl);

            HttpWebResponse response;
            try
            {
                WebResponse wr = await request.GetResponseAsync();
                response = wr as HttpWebResponse;
            }
            catch (WebException webex)
            {
                response = webex.Response as HttpWebResponse;
            }

            if (null == response)
                return null;

            UserObject user = null;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                user = JsonConvert.DeserializeObject<UserObject>(await reader.ReadToEndAsync());
            }

            response.Dispose();

            return user?.Id;
        }

        internal class UserObject
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }
    }

    public enum OAuthFlow
    {
        ImplicitGrant,
        AuthorizationCodeGrant
    }
}