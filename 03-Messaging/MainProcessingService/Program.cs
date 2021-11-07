using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SharedLibs;

namespace MainProcessingService
{
    class Program
    {
        private static FileProcessingService _fileProcessingService;

        static void Main(string[] args)
        {
            var filePersistenceService = new FilePersistenceService();
            var communicationService = new FileUploadCommunicationService();
            _fileProcessingService = new FileProcessingService(filePersistenceService, communicationService);
            _fileProcessingService.StartService();

            Console.ReadLine();
            Console.WriteLine("Finished");
        }
    }
}
