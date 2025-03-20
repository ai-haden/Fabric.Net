using Grpc.Net.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Collections.Generic;  // Add this

namespace Fabric.Net
{
    public class FabricClient
    {
        private readonly string _channelName;
        private readonly string _peerEndpoint;
        private readonly string _ordererEndpoint;
        private readonly X509Certificate2 _clientCert;
        private readonly X509Certificate2 _tlsCaCert;

        public FabricClient(string channelName, string peerEndpoint, string ordererEndpoint, string certPath, string keyPath, string tlsCaCertPath)
        {
            _channelName = channelName;
            _peerEndpoint = peerEndpoint;
            _ordererEndpoint = ordererEndpoint;
            string certPem = File.ReadAllText(certPath);
            string keyPem = File.ReadAllText(keyPath);
            string pem = $"-----BEGIN CERTIFICATE-----\n{certPem}\n-----END CERTIFICATE-----\n-----BEGIN PRIVATE KEY-----\n{keyPem}\n-----END PRIVATE KEY-----";
            _clientCert = X509Certificate2.CreateFromPem(pem);
            _tlsCaCert = new X509Certificate2(File.ReadAllBytes(tlsCaCertPath));
        }

        public async Task<string> InvokeChaincodeAsync(string chaincodeName, string function, params string[] args)
        {
            var handler = GetTlsHandler();
            var channel = GrpcChannel.ForAddress(_peerEndpoint, new GrpcChannelOptions { HttpHandler = handler });
            var client = new FabricService.FabricServiceClient(channel);

            var request = new TransactionProposalRequest
            {
                ChaincodeName = chaincodeName,
                Function = function,
                ChannelId = _channelName
            };
            request.Args.AddRange(args);

            var response = await client.ProposeAsync(request);
            if (!response.Status.Success)
                throw new Exception($"Proposal failed: {response.Status.Message}");

            var ordererChannel = GrpcChannel.ForAddress(_ordererEndpoint, new GrpcChannelOptions { HttpHandler = handler });
            var ordererClient = new OrdererService.OrdererServiceClient(ordererChannel);
            var commitRequest = new TransactionCommitRequest
            {
                TransactionId = response.TransactionId,
                ProposalResponses = { response }
            };
            var commitResponse = await ordererClient.CommitAsync(commitRequest);

            return commitResponse.TransactionId;
        }

        public async Task<string> QueryChaincodeAsync(string chaincodeName, string function, params string[] args)
        {
            var handler = GetTlsHandler();
            var channel = GrpcChannel.ForAddress(_peerEndpoint, new GrpcChannelOptions { HttpHandler = handler });
            var client = new FabricService.FabricServiceClient(channel);

            var request = new TransactionProposalRequest
            {
                ChaincodeName = chaincodeName,
                Function = function,
                ChannelId = _channelName
            };
            request.Args.AddRange(args);

            var response = await client.EvaluateAsync(request);  // New placeholder method
            if (!response.Status.Success)
                throw new Exception($"Query failed: {response.Status.Message}");

            return response.Result;  // Assuming Result holds the query output
        }

        private HttpClientHandler GetTlsHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(_clientCert);
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                chain.ChainPolicy.CustomTrustStore.Add(_tlsCaCert);
                chain.Build(cert);
                return true;
            };
            return handler;
        }
    }
}
