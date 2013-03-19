using System;
using System.Collections.Generic;
using Chat.Client.EventArguments;
using Chat.Client.Service;

namespace Chat.Client.Models
{
    public class MainChatModel : ChatModelBase, IMainChatModel
    {
        private bool _isOpen;

        public MainChatModel(ChatServiceClient proxy, string userName)
            : base(proxy, userName)
        {
            Proxy.NotifyReceived += ProxyOnNotifyReceived;
            Proxy.NotifyPersonalReceived += ProxyOnNotifyPersonalReceived;
        }

        private void ProxyOnNotifyPersonalReceived(object sender, NotifyPersonalReceivedEventArgs e)
        {
            if (!HasError(e) && NewPersonalChat != null)
                NewPersonalChat(this, new NewMessageEventArgs{UserName = e.from, Message = e.message});
        }

        private void ProxyOnNotifyReceived(object sender, NotifyReceivedEventArgs e)
        {
            if (!HasError(e))
            {
                if (e.from == "Chat")
                    OnRefreshUsers(e.message.Split(new[] {','}));
                else
                    OnNewMessage(e.from, e.message);
            }
        }

        private void OnRefreshUsers(IEnumerable<string> users)
        {
            if (RefreshUsers != null)
                RefreshUsers(this, new RefreshUsersEventArgs{Users = users});
        }

        #region Implementation of IMainChatModel

        public event EventHandler<NewMessageEventArgs> NewPersonalChat;
        public event EventHandler<RefreshUsersEventArgs> RefreshUsers;
        public event EventHandler Close;

        public void OpenChat()
        {
            OnNewSystemMessage("Attaching client...");
            Proxy.AttachClientCompleted += (sender, args) =>
            {
                if (!HasError(args))
                {
                    _isOpen = true;
                    OnNewSystemMessage("Client attached.");
                }
            };
            Proxy.AttachClientAsync(UserName);
        }

        public void CloseChat()
        {
            if (_isOpen)
            {
                _isOpen = false;
                OnNewSystemMessage("Detaching client...");
                Proxy.DetachClientCompleted +=
                    (sender, args) =>
                    {
                        if (!HasError(args))
                        {
                            OnNewSystemMessage("Client detached.");
                            Proxy.CloseCompleted += (o, eventArgs) =>
                            {
                                if (!HasError(eventArgs) && Close != null)
                                    Close(this, EventArgs.Empty);
                            };
                            Proxy.CloseAsync();
                        }
                    };
                Proxy.DetachClientAsync(UserName);
            }
        }

        public IChatModel GetNewPersonalChat(string userName)
        {
            return new PersonalChatModel(Proxy, userName);
        }

        #endregion

        #region Overrides of ChatModelBase

        public override void SendMessage(string from, string message)
        {
            Proxy.BroadcastAsync(from, message);
        }

        #endregion
    }
}
