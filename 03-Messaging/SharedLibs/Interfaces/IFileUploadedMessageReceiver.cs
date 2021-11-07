using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace SharedLibs.Interfaces
{
    public interface IFileUploadedMessageReceiver
    {
        void RegisterMessageHandler(Func<Message, CancellationToken, Task> messageHandler);
        Task CompleteMessage(Message message);
    }
}
