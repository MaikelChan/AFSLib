using System;
using System.IO;

namespace AFSLib
{
    public class FileEntry : Entry
    {
        private readonly FileInfo fileInfo;

        internal FileEntry(string fileNamePath, string entryName)
        {
            if (entryName == null)
            {
                entryName = string.Empty;
            }

            if (entryName.Length > AFS.MAX_ENTRY_NAME_LENGTH)
            {
                throw new ArgumentOutOfRangeException(nameof(entryName), $"Entry name can't be longer than {AFS.MAX_ENTRY_NAME_LENGTH} characters: \"{entryName}\".");
            }

            fileInfo = new FileInfo(fileNamePath);

            rawName = entryName;
            size = (uint)fileInfo.Length;
            lastWriteTime = fileInfo.LastWriteTime;
            unknown = (uint)fileInfo.Length;
        }

        internal override Stream GetStream()
        {
            return fileInfo.OpenRead();
        }
    }
}