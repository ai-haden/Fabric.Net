namespace Fabric.Net
{
    public class TransactionCommitRequest
    {
        public string TransactionId { get; set; }
        public List<TransactionProposalResponse> ProposalResponses { get; set; } = new List<TransactionProposalResponse>();  // Changed to List
    }
}