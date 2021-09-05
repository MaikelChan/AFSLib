using System;
using System.IO;

namespace AFSLib
{
    public class StreamEntry : Entry
    {
        private readonly Stream baseStream;
        private readonly uint baseStreamDataOffset;

        internal StreamEntry(Stream baseStream, StreamEntryInfo info)
        {
            if (info.RawName == null)
            {
                info.RawName = string.Empty;
            }

            if (info.RawName.Length > AFS.MAX_ENTRY_NAME_LENGTH)
            {
                throw new ArgumentOutOfRangeException(nameof(info.RawName), $"Entry name can't be longer than {AFS.MAX_ENTRY_NAME_LENGTH} characters: \"{info.RawName}\".");
            }

            this.baseStream = baseStream;
            baseStreamDataOffset = info.Offset;

            rawName = info.RawName;
            size = info.Size;
            lastWriteTime = info.LastWriteTime;
            unknown = info.Unknown;
        }

        internal override Stream GetStream()
        {
            baseStream.Position = baseStreamDataOffset;
            return new SubStream(baseStream, 0, Size, true);
        }
    }
}