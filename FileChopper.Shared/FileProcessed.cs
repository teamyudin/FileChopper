using System;
using NServiceBus;

namespace FileChopper.Shared
{
    public class FileProcessed: IMessage
    {
        public Guid FileId { get; set; }
        public string OutputDirectory { get; set; }
    }
}
