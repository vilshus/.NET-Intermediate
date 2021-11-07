using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SharedLibs;
using SharedLibs.Interfaces;

namespace MainProcessingService
{
    public class FileProcessingService
    {
        private const string LocalStorageFolder = @"..\..\..\..\Local storage";

        private readonly IFileDownloader _fileDownloadingService;
        private readonly IFileUploadedMessageReceiver _fileUploadedMessageReceivingService;

        public FileProcessingService(IFileDownloader fileDownloadingService, IFileUploadedMessageReceiver fileUploadedMessageReceivingService)
        {
            _fileDownloadingService = fileDownloadingService;
            _fileUploadedMessageReceivingService = fileUploadedMessageReceivingService;
        }

        public void StartService()
        {
            _fileUploadedMessageReceivingService.RegisterMessageHandler(ProcessMessagesAsync);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Message received: {Encoding.UTF8.GetString(message.Body)}");

            var fileId = Guid.Parse(Encoding.UTF8.GetString(message.Body));
            var fileFullName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), LocalStorageFolder, $"{fileId}{Constants.FileType}"));
            var file = File.OpenWrite(fileFullName);

            try
            {
                await using (file)
                {
                    await _fileDownloadingService.DownloadFile(fileId, file);
                    await _fileUploadedMessageReceivingService.CompleteMessage(message);
                    Console.WriteLine($"File downloaded. ID: {fileId}; Path: {fileFullName};");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _fileUploadedMessageReceivingService.CompleteMessage(message);
            }
        }
    }
}
