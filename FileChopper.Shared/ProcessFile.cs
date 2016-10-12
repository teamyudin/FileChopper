using System;
using NServiceBus;

namespace FileChopper.Shared
{
    public class ProcessFile: ICommand
    {
        public Guid FileId { get; set; }
        public string Path { get; set; }
        public int NumberOfLinesPerOutputFile { get; set; }
    }
}