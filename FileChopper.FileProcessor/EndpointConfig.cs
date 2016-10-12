using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace FileChopper.FileProcessor
{
    public class ProvideConfiguration : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaximumConcurrencyLevel = 2,
                MaximumMessageThroughputPerSecond = 10
            };
        }
    }

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            Configure.Transactions.Disable();

            var configure = Configure.With();
            configure.DefineEndpointName("FileChopper.FileProcessor");
            configure.DefaultBuilder();

            configure.RavenPersistence();
            configure.UseTransport<Msmq>();

        }
    }
}
