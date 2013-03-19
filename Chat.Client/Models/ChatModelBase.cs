using System;
using System.ComponentModel;
using Chat.Client.EventArguments;
using Chat.Client.Service;

namespace Chat.Client.Models
{
    public abstract class ChatModelBase : IChatModel
    {
        protected readonly ChatServiceClient Proxy;

        protected ChatModelBase(ChatServiceClient proxy, string userName)
        {
            UserName = userName;
            Proxy = proxy;
        }

        protected bool HasError(AsyncCompletedEventArgs args)
        {
            var hasError = args.Error != null;
            if (hasError)
                OnNewSystemMessage(args.Error.Message);
            return hasError;
        }

        protected void OnNewMessage(string message)
        {
            if (NewMessage != null)
                NewMessage(this, new NewMessageEventArgs { UserName = UserName, Message = message });
        }

        protected void OnNewMessage(string userName, string message)
        {
            if (NewMessage != null)
                NewMessage(this, new NewMessageEventArgs { UserName = userName, Message = message });
        }

        protected void OnNewSystemMessage(string message)
        {
            if (NewMessage != null)
                NewMessage(this, new NewMessageEventArgs { UserName = "[Chat]", Message = message });
        }

        #region Implementation of IChatModel

        public string UserName { get; set; }

        public event EventHandler<NewMessageEventArgs> NewMessage;

        public abstract void SendMessage(string from, string message);

        #endregion
    }
}
