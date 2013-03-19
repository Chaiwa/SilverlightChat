using System.Collections.Generic;

namespace Chat.Client.EventArguments
{
    public class RefreshUsersEventArgs : System.EventArgs
    {
        public IEnumerable<string> Users;
    }
}