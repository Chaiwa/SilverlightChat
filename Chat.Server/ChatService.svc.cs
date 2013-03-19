using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Chat.Server
{
    [ServiceContract(Namespace = "", CallbackContract = typeof(IChatClient))]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService
    {
        private readonly object _lock = new object();

        private SynchronizedCollection<ClientEntry> _clients = new SynchronizedCollection<ClientEntry>();

        [OperationContract]
        public void AttachClient(string name)
        {
            var client = OperationContext.Current.GetCallbackChannel<IChatClient>();
            var clientEntry = new ClientEntry {Name = name, Client = client};
            _clients.Add(clientEntry);
            NotifyUsersChanged();
        }

        [OperationContract]
        public void DetachClient(string name)
        {
            _clients = new SynchronizedCollection<ClientEntry>(syncRoot: _lock, list: _clients.Where(x => x.Name != name));
            NotifyUsersChanged();
        }

        [OperationContract]
        public void Broadcast(string from, string message)
        {
            foreach (var client in _clients.Where(c => c.Name != from))
            {
                client.Client.Notify(from, message);
            }
        }

        [OperationContract]
        public void SendMessage(string from, string to, string message)
        {
            var client = _clients.SingleOrDefault(c => c.Name == to);
            if (client != null)
                client.Client.NotifyPersonal(from, message);
        }

        private void NotifyUsersChanged()
        {
            foreach (var clientEntry in _clients)
            {
                var entry = clientEntry;
                var users = _clients.Where(x => x.Name != entry.Name).Select(x => x.Name);
                clientEntry.Client.Notify("Chat", string.Join(",", users));
            }
        }
    }
}
