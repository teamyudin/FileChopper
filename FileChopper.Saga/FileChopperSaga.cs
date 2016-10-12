using System;
using FileChopper.Shared;
using NServiceBus;
using NServiceBus.Saga;

namespace FileChopper.Saga
{
    public class FileChopperSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }

    public class FileChopperSaga: Saga<FileChopperSagaData>,
        IAmStartedByMessages<ProcessFile>,
        IHandleMessages<FileProcessed>,
        IHandleTimeouts<CancelProcessingFile>
    {

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<ProcessFile>(message => message.FileId).ToSaga(sagaData => sagaData.FileId);
            ConfigureMapping<FileProcessed>(message => message.FileId).ToSaga(sagaData => sagaData.FileId);
        }

        public void Handle(ProcessFile message)
        {
            Data.FileId = message.FileId;

            Bus.Send("FileChopper.FileProcessor", message);

            RequestTimeout<CancelProcessingFile>(TimeSpan.FromHours(1));
        }

        public void Timeout(CancelProcessingFile message)
        {
            MarkAsComplete();
        }

        public void Handle(FileProcessed message)
        {
            MarkAsComplete();
        }
    }
}
