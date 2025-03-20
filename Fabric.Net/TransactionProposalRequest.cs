namespace Fabric.Net
{
    public class TransactionProposalRequest
    {
        public string ChaincodeName { get; set; }
        public string Function { get; set; }
        public string ChannelId { get; set; }
        public List<string> Args { get; set; } = new List<string>();  // Changed to List<string>
    }
}