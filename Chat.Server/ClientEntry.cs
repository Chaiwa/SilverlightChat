namespace Chat.Server
{
    public class ClientEntry
    {
        public string Name { get; set; }

        public IChatClient Client { get; set; }
    }
}