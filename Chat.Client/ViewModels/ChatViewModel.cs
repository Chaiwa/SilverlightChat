using System.Collections.ObjectModel;
using Chat.Client.Models;

namespace Chat.Client.ViewModels
{
    public abstract class ChatViewModel
    {
        public IChatModel Model { get; private set; }
        public ObservableCollection<string> Messages { get; set; }

        public string UserName
        {
            get { return Model.UserName; }
        }

        protected ChatViewModel(IChatModel model)
        {
            Model = model;
            Messages = new ObservableCollection<string>();

            Model.NewMessage += (sender, args) => InsertMessage(args.UserName, args.Message);
        }

        public virtual void SendMessage(string text)
        {
            InsertMessage(Model.UserName, text);
            Model.SendMessage(Model.UserName, text);
        }

        protected void InsertMessage(string from, string message)
        {
            Messages.Add(MessageHelper.ComposeNewMessageString(@from, message));
        }
    }
}