using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SharedLibs.Interfaces;

namespace SharedLibs
{
    public class FileUploadCommunicationService : IFileUploadedMessageSender, IFileUploadedMessageReceiver
    {
        private const string ConnectionString = "";
        private const string TopicName = "fileprocessing";
        private const string SubscriptionName = "file-uploaded-signal";
        private static ITopicClient _topicClient;
        private static ISubscriptionClient _subscriptionClient;

        public FileUploadCommunicationService(ITopicClient topicClient = null, ISubscriptionClient subscriptionClient = null)
        {
            if (topicClient == null || subscriptionClient == null)
            {
                _topicClient = new TopicClient(ConnectionString, TopicName);
                _subscriptionClient = new SubscriptionClient(ConnectionString, TopicName, SubscriptionName);
            }
            else
            {
                _topicClient = topicClient;
                _subscriptionClient = subscriptionClient;
            }
        }

        public async Task SendFileUploadedMessage(Guid fileId)
        {
            var message = new Message(Encoding.UTF8.GetBytes(fileId.ToString()));
            await _topicClient.SendAsync(message);
        }

        public void RegisterMessageHandler(Func<Message, CancellationToken, Task> messageHandler)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
                MaxAutoRenewDuration = TimeSpan.FromMinutes(15)
            };

            _subscriptionClient.RegisterMessageHandler(messageHandler, messageHandlerOptions);
        }

        public async Task CompleteMessage(Message message)
        {
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        public async Task CloseConnections()
        {
            await _topicClient.CloseAsync();
            await _subscriptionClient.CloseAsync();
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}");
            return Task.CompletedTask;
        }
    }
}
