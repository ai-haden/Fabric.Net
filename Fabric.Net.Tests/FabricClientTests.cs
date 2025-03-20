using System.Security.Cryptography.X509Certificates;

namespace Fabric.Net.Tests
{
    public class FabricClientTests
    {
        [Fact]
        public async Task InvokeChaincodeAsync_SuccessfulProposalAndCommit_ReturnsTransactionId()
        {
            // Arrange
            var certPath = "test-cert.pem";
            var keyPath = "test-key.pem";
            var tlsCaCertPath = "test-tls-ca.crt";
            File.WriteAllText(certPath, "fake-cert");
            File.WriteAllText(keyPath, "fake-key");
            File.WriteAllText(tlsCaCertPath, new X509Certificate2().Export(X509ContentType.Cert).ToString());

            var client = new FabricClient(
                channelName: "haden",
                peerEndpoint: "grpcs://slowclock.com:7051",
                ordererEndpoint: "grpcs://slowclock.com:7050",
                certPath: certPath,
                keyPath: keyPath,
                tlsCaCertPath: tlsCaCertPath
            );

            // Act
            string txId = await client.InvokeChaincodeAsync("batterylevelcc", "reportBattery", "Robot1", "9000", "2025-03-20T12:00:00Z");

            // Assert
            Assert.NotNull(txId);
            Assert.NotEmpty(txId);

            // Cleanup
            File.Delete(certPath);
            File.Delete(keyPath);
            File.Delete(tlsCaCertPath);
        }
    }
}