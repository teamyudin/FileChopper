using System;
using System.IO;
using System.Reflection;
using FileChopper.Shared;
using NServiceBus;

namespace FileChopper.FileProcessor
{
    public class ProcessFileHandler: IHandleMessages<ProcessFile>
    {
        private readonly IBus _bus;
        
        public ProcessFileHandler(IBus bus)
        {
            _bus = bus;

        }

        public void Handle(ProcessFile message)
        {
            var outputDirectory = GetOutputDirectory(message.FileId);

            var fileSplitter = new FileSplitter();

            fileSplitter.SplitFile(message.Path, outputDirectory, message.NumberOfLinesPerOutputFile);

            _bus.Reply(new FileProcessed
            {
                FileId = message.FileId,
                OutputDirectory = outputDirectory
            });
        }

        private static string GetOutputDirectory(Guid fileId)
        {
            var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(executableLocation, @"output/" + fileId);
        }
    }
}
