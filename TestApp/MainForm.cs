using System;
using System.IO;
using System.Windows.Forms;

using MicrosoftAccount.WindowsForms;

using Newtonsoft.Json;

namespace TestApp
{
    public partial class MainForm : Form
    {
        public class LocalSettings
        {
            public string ClientId { get; set; }
            public string Scope { get; set; }
            public AppTokenResult TokenResult { get; set; }
        }

        private string ResultsLocation => $"{AppDomain.CurrentDomain.BaseDirectory}result.json";

        public MainForm() => InitializeComponent();

        private void OnLoad(object sender, EventArgs e)
        {
            if (File.Exists(ResultsLocation))
            {
                var fileText = File.ReadAllText(ResultsLocation);
                var localSettings = JsonConvert.DeserializeObject<LocalSettings>(fileText);
                tbClientId.Text = localSettings.ClientId;
                tbScope.Text = localSettings.Scope;
                tbRefreshToken.Text = localSettings.TokenResult.RefreshToken;
            }
        }

        private async void OnLogin(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbClientId.Text))
            {
                tbStatus.Text = "Client Id is empty!";
                return;
            }

            if (string.IsNullOrEmpty(tbScope.Text))
            {
                tbStatus.Text = "Scope it empty!";
                return;
            }

            var scopes = tbScope.Text.Split(' ');

            var authResult = await MicrosoftAccountOAuth.LoginAuthorizationCodeFlowAsync(tbClientId.Text, scopes, true);

            if (authResult?.IsError == true)
            {
                tbStatus.Text = authResult.Error;
                return;
            }

            SaveSettings(authResult);
        }

        private async void OnRefresh(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbClientId.Text))
            {
                tbStatus.Text = "Client Id is empty!";
                return;
            }

            if (string.IsNullOrEmpty(tbRefreshToken.Text))
            {
                tbStatus.Text = "Refresh token it empty!";
                return;
            }

            var authResult = await MicrosoftAccountOAuth.RedeemRefreshTokenAsync(tbClientId.Text, tbRefreshToken.Text);

            if (authResult.IsError)
            {
                tbStatus.Text = authResult.Error;
                return;
            }

            SaveSettings(authResult);
        }

        private void SaveSettings(AppTokenResult authResult)
        {
            var localSettings = new LocalSettings() { ClientId = tbClientId.Text, Scope = tbScope.Text, TokenResult = authResult };
            File.WriteAllText(ResultsLocation, JsonConvert.SerializeObject(localSettings));

            tbRefreshToken.Text = authResult.RefreshToken;
        }
    }
}
