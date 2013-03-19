using Chat.Client.Service;

namespace Chat.Client.Models
{
    public class PersonalChatModel : ChatModelBase
    {
        public PersonalChatModel(ChatServiceClient proxy, string userName)
            : base(proxy, userName)
        {
            Proxy.NotifyPersonalReceived +=
                (sender, args) =>
                    {
                        if (!HasError(args))
                            OnNewMessage(args.message);
                    };
        }

        #region Overrides of ChatModelBase

        public override void SendMessage(string from, string message)
        {
            Proxy.SendMessageAsync(from, UserName, message);
        }

        #endregion
    }
}
