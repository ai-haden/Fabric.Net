using Fabric.Net;

public class BatteryReportingTests
{
    private readonly FabricClient _client;

    public BatteryReportingTests()
    {
        _client = new FabricClient(
            channelName: "haden",
            peerEndpoint: "grpcs://slowclock.com:7051",
            ordererEndpoint: "grpcs://slowclock.com:7050",
            certPath: "wallet/admin-cert.pem",
            keyPath: "wallet/admin-key.pem",
            tlsCaCertPath: "wallet/tls-ca.crt"
        );
    }

    [Fact]
    public async Task ReportBattery_ReportsAndQueriesSuccessfully()
    {
        string robotId = "Robot1";
        string batteryLevel = "9000";
        string timestamp = "2025-03-20T12:00:00Z";
        string expectedData = $"Robot {robotId} battery: {batteryLevel} mV at {timestamp}";

        string txId = await _client.InvokeChaincodeAsync("batterylevelcc", "reportBattery", robotId, batteryLevel, timestamp);
        Assert.NotNull(txId);
        Assert.NotEmpty(txId);

        string queryResult = await _client.QueryChaincodeAsync("batterylevelcc", "queryBattery", robotId);
        Assert.Equal(expectedData, queryResult);
    }
}