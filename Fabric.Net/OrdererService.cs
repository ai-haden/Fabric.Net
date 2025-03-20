using Grpc.Net.Client;

namespace Fabric.Net
{
    public class OrdererService
    {
        public class OrdererServiceClient
        {
            private readonly GrpcChannel _channel;
            public OrdererServiceClient(GrpcChannel channel) => _channel = channel;
            public async Task<TransactionCommitResponse> CommitAsync(TransactionCommitRequest request)
            {
                // Simulate commit
                await Task.Delay(100); // Placeholder
                return new TransactionCommitResponse { TransactionId = request.TransactionId };
            }
        }
    }
}