using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Core.FileStorage
{
    public interface IFileStorage : IFileInfo
    {
        string GetFileType();

        Stream OpenRead();

        Stream OpenWrite();

        Stream CreateFile();
    }
}