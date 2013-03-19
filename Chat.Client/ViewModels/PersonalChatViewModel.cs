using Chat.Client.Models;

namespace Chat.Client.ViewModels
{
    public class PersonalChatViewModel : ChatViewModel
    {
        private readonly MainPageViewModel _parent;

        public PersonalChatViewModel(IChatModel model, MainPageViewModel parent)
            : base(model)
        {
            _parent = parent;
        }

        public void Close()
        {
            _parent.ClosePersonalChat(Model.UserName);
        }

        public override void SendMessage(string text)
        {
            InsertMessage(_parent.Model.UserName, text);
            Model.SendMessage(_parent.Model.UserName, text);
        }
    }
}