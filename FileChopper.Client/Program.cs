using System;
using System.IO;
using FileChopper.Shared;
using NServiceBus;
using System.Reflection;

namespace FileChopper.Client
{
    class Program
    {
        static void Main()
        {
            Console.Title = "FileChopper.Client";
            Configure.Serialization.Json();
            var configure = Configure.With();
            configure.DefineEndpointName("FileChopper.Client");
            configure.DefaultBuilder();
            configure.RavenPersistence();
            configure.UseTransport<Msmq>();

            using (var startableBus = configure.UnicastBus().CreateBus())
            {
                var bus = startableBus.Start(() => configure.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

                ProcessFile(bus);
            }
        }

        private static void ProcessFile(IBus bus)
        {
            Console.WriteLine("Press enter to start new import");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        Start(bus);
                        break;
                    default:
                        return;
                }
            }
        }

        private static void Start(IBus bus)
        {
            var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (executableLocation == null) return;

            var filePath = Path.Combine(executableLocation, @"data\data.txt");

            var message = new ProcessFile
            {
                FileId = Guid.NewGuid(),
                Path = filePath,
                NumberOfLinesPerOutputFile = 20
            };

            bus.Send("FileChopper.Saga", message);
        }
    }
}
