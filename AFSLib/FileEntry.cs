using System.IO;

namespace AFSLib
{
    public sealed class FileEntry : DataEntry
    {
        private readonly FileInfo fileInfo;

        internal FileEntry(string fileNamePath, string entryName)
        {
            fileInfo = new FileInfo(fileNamePath);

            Name = entryName;
            Size = (uint)fileInfo.Length;
            LastWriteTime = fileInfo.LastWriteTime;
            Unknown = (uint)fileInfo.Length;
        }

        internal override Stream GetStream()
        {
            return fileInfo.OpenRead();
        }
    }
}