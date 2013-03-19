namespace Chat.Client.EventArguments
{
    public class NewMessageEventArgs : System.EventArgs
    {
        public string UserName;
        public string Message;
    }
}