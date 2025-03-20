## Fabric.Net

Hyperledger Fabric SDK for dotnet.

### Background

Seeing that the dotnet C# projects functioning as a Hyperledger Fabric SDK were woefully incomplete or _way_ overenthusiastic, I created a simple SDK with a test to poll the battery value of a remote robot at the ledger.

### Usage

In the test directory, copy the correct certs:

```
mkdir wallet
cp /home/cartheur/go/src/github.com/cartheur/fabric-samples/test-network/organizations/peerOrganizations/org1.example.com/users/Admin@org1.example.com/msp/signcerts/* wallet/admin-cert.pem
cp /home/cartheur/go/src/github.com/cartheur/fabric-samples/test-network/organizations/peerOrganizations/org1.example.com/users/Admin@org1.example.com/msp/keystore/* wallet/admin-key.pem
cp /home/cartheur/go/src/github.com/cartheur/fabric-samples/test-network/organizations/peerOrganizations/org1.example.com/peers/peer0.org1.example.com/tls/ca.crt wallet/tls-ca.crt
```

### Results

Todo.