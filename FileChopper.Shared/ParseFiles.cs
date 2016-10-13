using System;
using NServiceBus;

namespace FileChopper.Shared
{
    public class ParseFiles: ICommand
    {
        public Guid FileId { get; set; }
        public string Path { get; set; }
    }
}
