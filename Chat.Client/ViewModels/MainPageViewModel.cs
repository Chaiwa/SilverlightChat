using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chat.Client.EventArguments;
using Chat.Client.Models;

namespace Chat.Client.ViewModels
{
    public class MainPageViewModel : ChatViewModel
    {
        private readonly Dictionary<string, PersonalChatViewModel> _personalChats;
        
        public ObservableCollection<string> Users { get; set; }

        public event EventHandler<PersonalChatEventArgs> ClosePersonal;
        public event EventHandler<PersonalChatEventArgs> OpenPersonal;

        public MainPageViewModel(IMainChatModel model) : base(model)
        {
            Users = new ObservableCollection<string>();
            _personalChats = new Dictionary<string, PersonalChatViewModel>();

            model.RefreshUsers += (sender, args) => RefreshUsers(args.Users);
            model.NewPersonalChat += ModelOnNewPersonalChat;
        }

        public void Close(Action onClose)
        {
            var model = (IMainChatModel) Model;
            model.Close += (sender, args) => onClose();
            model.CloseChat();
        }

        public void Open()
        {
            var model = (IMainChatModel) Model;
            model.OpenChat();
        }

        public PersonalChatViewModel GetPersonalChat(string userName)
        {
            if (!_personalChats.ContainsKey(userName))
            {
                var model = (IMainChatModel) Model;
                _personalChats.Add(userName, new PersonalChatViewModel(model.GetNewPersonalChat(userName), parent: this));
            }
            return _personalChats[userName];
        }

        public void ClosePersonalChat(string userName)
        {
            _personalChats.Remove(userName);
            if (ClosePersonal != null)
                ClosePersonal(this, new PersonalChatEventArgs { UserName = userName });
        }


        private void ModelOnNewPersonalChat(object sender, NewMessageEventArgs e)
        {
            if (!_personalChats.ContainsKey(e.UserName))
            {
                var personalChat = GetPersonalChat(e.UserName);
                personalChat.Messages.Add(MessageHelper.ComposeNewMessageString(e.UserName, e.Message));
                if (OpenPersonal != null)
                    OpenPersonal(this, new PersonalChatEventArgs{UserName = e.UserName});
            }
        }

        private void RefreshUsers(IEnumerable<string> users)
        {
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }
}
