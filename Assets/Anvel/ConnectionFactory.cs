using Thrift.Protocol;
using Thrift.Transport;
using AnvelApi;

namespace CAVS.Anvel
{

    public static class ConnectionFactory
    {

        public static AnvelControlService.Client CreateConnection(ClientConnectionToken connectionToken)
        {
            var transport = new TSocket(connectionToken.GetIpAddress(), connectionToken.GetPort());
            var client = new AnvelControlService.Client(new TBinaryProtocol(transport));
            transport.Open();
            transport.TcpClient.NoDelay = true;
            return client;
        }

        public static AnvelControlService.Client CreateConnection()
        {
            var transport = new TSocket("127.0.0.1", 9094);
            var client = new AnvelControlService.Client(new TBinaryProtocol(transport));
            transport.Open();
            transport.TcpClient.NoDelay = true;
            return client;
        }

    }

}