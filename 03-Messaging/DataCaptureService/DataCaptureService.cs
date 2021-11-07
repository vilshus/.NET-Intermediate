using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SharedLibs;
using SharedLibs.Interfaces;

namespace DataCaptureService
{
    public class DataCaptureService
    {
        private const string SourceFolder = @"..\..\..\..\Data capture files";

        // To avoid deleting the files from the folder
        // the info about stored files is saved to remember which are already uploaded.
        private readonly Dictionary<string, Guid> _filesUploaded = new Dictionary<string, Guid>();

        private readonly IFileUploader _fileUploadingService;
        private readonly IFileUploadedMessageSender _fileUploadedMessageSendingService;

        public DataCaptureService(IFileUploader fileUploadingService, IFileUploadedMessageSender fileUploadedMessageSendingService)
        {
            _fileUploadingService = fileUploadingService;
            _fileUploadedMessageSendingService = fileUploadedMessageSendingService;
        }

        public async Task StartService()
        {
            while (true)
            {
                await UploadNewFiles();
                await Task.Delay(5000);
            }
        }

        private async Task UploadNewFiles()
        {
            var localFolder = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), SourceFolder));
            var files = localFolder.GetFiles();
            foreach (var fileInfo in files)
            {
                if (Path.GetExtension(fileInfo.FullName) != Constants.FileType)
                {
                    continue;
                }

                if (_filesUploaded.ContainsKey(fileInfo.Name))
                {
                    continue;
                }

                var file = File.OpenRead(fileInfo.FullName);

                await using (file)
                {
                    var fileId = Guid.NewGuid();

                    await _fileUploadingService.UploadFileAsync(fileId, file);
                    _filesUploaded.Add(fileInfo.Name, fileId);
                    
                    await _fileUploadedMessageSendingService.SendFileUploadedMessage(fileId);
                    Console.WriteLine($"File uploaded. ID: {fileId}; Path: {fileInfo.FullName};");
                }
            }
        }
    }
}
