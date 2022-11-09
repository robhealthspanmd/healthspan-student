using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.FileSystem
{
    public interface IFileSystemManager
    {
        void SaveFile(string containerName, string fileName, byte[] fileData);
        byte[] GetFile(string containerName, string fileName);
    }
}
