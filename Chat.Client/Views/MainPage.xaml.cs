using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chat.Client.ViewModels;
using System.Linq;

namespace Chat.Client.Views
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
                          {
                              var viewModel = (MainPageViewModel) DataContext;
                              viewModel.Open();
                              viewModel.OpenPersonal += (o, eventArgs) => AddTab(eventArgs.UserName);
                              viewModel.ClosePersonal += (o, eventArgs) => CloseTab(eventArgs.UserName);
                          };
        }

        #region Event handlers

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainPageViewModel) DataContext;
            viewModel.Close(() =>
                                {
                                    var app = (App) Application.Current;
                                    app.RedirectTo(new Login());
                                });
        }

        private void UserName_Click(object sender, RoutedEventArgs e)
        {
            var userLink = sender as HyperlinkButton;
            if (userLink != null)
            {
                var userName = userLink.Content as string;
                var tabItem = FindTabByTag(userName);
                if (tabItem == null)
                {
                    AddTab(userName);
                }
                else
                {
                    tabControl.SelectedItem = tabItem;
                }
            }
        }

        #endregion

        private TabItem FindTabByTag(string tag)
        {
            return tabControl.Items.OfType<TabItem>().SingleOrDefault(t => t.Tag.ToString() == tag);
        }

        private void AddTab(string userName)
        {
            var viewModel = (MainPageViewModel) DataContext;
            var personalChat = viewModel.GetPersonalChat(userName);

            var tabHeader = new TabHeader {DataContext = personalChat};
            var tabContent = new TabContent {DataContext = personalChat};
            var tabItem = new TabItem {Header = tabHeader, Content = tabContent, Tag = userName};
            tabControl.Items.Add(tabItem);
            tabControl.SelectedItem = tabItem;
        }

        private void CloseTab(string userName)
        {
            var closingTabItem = FindTabByTag(userName);
            var mainTabItem = FindTabByTag("main");
            tabControl.SelectedItem = mainTabItem;
            tabControl.Items.Remove(closingTabItem);
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(TxtMessage.Text))
            {
                var viewModel = (ChatViewModel)DataContext;
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
    }
}
