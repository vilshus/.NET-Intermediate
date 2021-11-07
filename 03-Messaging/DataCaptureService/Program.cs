using System;
using System.Threading.Tasks;
using SharedLibs;

namespace DataCaptureService
{
    class Program
    {
        private static DataCaptureService _dataCaptureService;

        static void Main(string[] args)
        {
            var filePersistenceService = new FilePersistenceService();
            var communicationService = new FileUploadCommunicationService();
            _dataCaptureService = new DataCaptureService(filePersistenceService, communicationService);
            Task.Run(() => _dataCaptureService.StartService());

            Console.ReadLine();
            Console.WriteLine("Finished");
        }
    }
}
