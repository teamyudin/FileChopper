using NServiceBus;

namespace FileChopper.Saga
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            var configure = Configure.With();
            configure.DefineEndpointName("FileChopper.Saga");
            configure.DefaultBuilder();
            configure.RavenPersistence();
            configure.UseTransport<Msmq>();
        }
    }
}
