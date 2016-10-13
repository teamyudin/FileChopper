using System;
using System.IO;
using FileChopper.Shared;
using NServiceBus;

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
            var fileId = Guid.NewGuid();
            var filePath = Path.Combine(Path.GetTempPath(), fileId + ".txt");

            CreateFile(100, filePath);

            var message = new ProcessFile
            {
                FileId = fileId,
                Path = filePath,
                NumberOfLinesPerOutputFile = 20
            };

            bus.Send("FileChopper.Saga", message);
        }

        private static void CreateFile(int numberOfLines, string filePath)
        {
            const string row = "asasasashjhhhasdhfalsfdhasfhashfashflaksjhfaksjhflksahflksahfksljahflaskhfaskljhflksajhflkasjhfkj";

            using (var writer = File.CreateText(filePath))
            {
                for (var i = 0; i < numberOfLines; i++)
                {
                    writer.WriteLine(row);
                }
            }
        }
    }
}
