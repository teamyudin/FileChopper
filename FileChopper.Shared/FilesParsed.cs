using System;
using NServiceBus;

namespace FileChopper.Shared
{
    public class FilesParsed: IMessage
    {
        public Guid FileId { get; set; }
    }
}
