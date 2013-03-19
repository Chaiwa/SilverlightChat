using System.Windows;
using System.Windows.Controls;
using Chat.Client.ViewModels;

namespace Chat.Client.Views
{
    public partial class TabHeader : UserControl
    {
        public TabHeader()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (PersonalChatViewModel) DataContext;
            viewModel.Close();
        }
    }
}
