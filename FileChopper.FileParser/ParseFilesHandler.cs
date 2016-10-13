using System.IO;
using FileChopper.Shared;
using NServiceBus;

namespace FileChopper.FileParser
{
    public class ParseFilesHandler: IHandleMessages<ParseFiles>
    {
        private readonly IBus _bus;

        public ParseFilesHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(ParseFiles message)
        {
            var files = Directory.GetFiles(message.Path);

            foreach (var file in files)
            {
                //Do something with the file
                File.Delete(file);
            }

            Directory.Delete(message.Path);

            _bus.Reply(new FilesParsed {FileId = message.FileId});
        }
    }
}
