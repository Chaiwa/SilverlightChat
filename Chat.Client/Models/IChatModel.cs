using System;
using Chat.Client.EventArguments;

namespace Chat.Client.Models
{
    public interface IChatModel
    {
        string UserName { get; set; }

        event EventHandler<NewMessageEventArgs> NewMessage;

        void SendMessage(string from, string message);
    }
}