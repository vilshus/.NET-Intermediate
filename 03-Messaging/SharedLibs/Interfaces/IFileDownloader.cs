using System;
using System.IO;
using System.Threading.Tasks;

namespace SharedLibs.Interfaces
{
    public interface IFileDownloader
    {
        Task DownloadFile(Guid fileId, Stream file);
    }
}
