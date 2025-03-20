namespace Fabric.Net
{
    public class TransactionProposalResponse
    {
        public string TransactionId { get; set; }
        public string Result { get; set; }  // Added for query results
        public Status Status { get; set; }
    }
}