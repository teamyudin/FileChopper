using System;
using NServiceBus;

namespace FileChopper.Shared
{
    public class CancelProcessingFile: IMessage
    {
        public Guid FileId { get; set; }
    }
}
