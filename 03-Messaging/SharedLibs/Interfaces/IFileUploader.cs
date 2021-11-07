using System;
using System.IO;
using System.Threading.Tasks;

namespace SharedLibs.Interfaces
{
    public interface IFileUploader
    {
        Task UploadFileAsync(Guid fileId, Stream file);
    }
}
