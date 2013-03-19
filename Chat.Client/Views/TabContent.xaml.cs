using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chat.Client.ViewModels;

namespace Chat.Client.Views
{
    public partial class TabContent : UserControl
    {
        public TabContent()
        {
            InitializeComponent();
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(TxtMessage.Text))
            {
                var viewModel = (ChatViewModel) DataContext;
                viewModel.SendMessage(TxtMessage.Text);
                TxtMessage.Text = string.Empty;
                SetScrollBarToBottom();
            }
        }

        private void SetScrollBarToBottom()
        {
            SvwrMessages.UpdateLayout();
            SvwrMessages.ScrollToVerticalOffset(double.MaxValue);
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
    }
}
