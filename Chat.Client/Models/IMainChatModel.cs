using System;
using Chat.Client.EventArguments;

namespace Chat.Client.Models
{
    public interface IMainChatModel : IChatModel
    {
        event EventHandler<NewMessageEventArgs> NewPersonalChat;
        event EventHandler<RefreshUsersEventArgs> RefreshUsers;
        event EventHandler Close; 

        void OpenChat();
        void CloseChat();
        IChatModel GetNewPersonalChat(string userName);
    }
}