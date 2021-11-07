using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibs.Interfaces
{
    public interface IFileUploadedMessageSender
    {
        Task SendFileUploadedMessage(Guid fileId);
    }
}
