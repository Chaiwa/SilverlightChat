using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chat.Client.Models;
using Chat.Client.Service;
using Chat.Client.ViewModels;

namespace Chat.Client.Views
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        #region Event handlers

        private void TxtUserName_MouseEnter(object sender, MouseEventArgs e)
        {
            HideUserNameError();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateUserNameNotEmpty())
            {
                TxtbUserNameRequired.Visibility = Visibility.Visible;
                return;
            }
            var serviceClient = new ChatServiceClient(
                new PollingDuplexHttpBinding { DuplexMode = PollingDuplexMode.MultipleMessagesPerPoll },
                new EndpointAddress("../ChatService.svc"));
            var model = new MainChatModel(serviceClient, TxtUserName.Text);
            var mainPageViewModel = new MainPageViewModel(model);
            var mainPage = new MainPage {DataContext = mainPageViewModel };
            var app = (App) Application.Current;
            app.RedirectTo(mainPage);
        }

        #endregion

        private bool ValidateUserNameNotEmpty()
        {
            return !string.IsNullOrEmpty(TxtUserName.Text) && !string.IsNullOrWhiteSpace(TxtUserName.Text);
        }

        private void HideUserNameError()
        {
            if (TxtbUserNameRequired.Visibility == Visibility.Visible)
                TxtbUserNameRequired.Visibility = Visibility.Collapsed;
        }
    }
}
