using System;

namespace Chat.Client
{
    public static class MessageHelper
    {
        public static string ComposeNewMessageString(string @from, string message)
        {
            return string.Format("{0} {1:HH.mm}: {2}", @from, DateTime.Now, message);
        }
    }
}
