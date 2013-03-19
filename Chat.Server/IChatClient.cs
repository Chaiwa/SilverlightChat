using System.ServiceModel;

namespace Chat.Server
{
    [ServiceContract]
    public interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void Notify(string from, string message);

        [OperationContract(IsOneWay = true)]
        void NotifyPersonal(string from, string message);
    }
}
