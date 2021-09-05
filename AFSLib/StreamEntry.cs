using System.IO;

namespace AFSLib
{
    public sealed class StreamEntry : Entry
    {
        private readonly Stream baseStream;
        private readonly uint baseStreamDataOffset;

        internal StreamEntry(Stream baseStream, StreamEntryInfo info)
        {
            this.baseStream = baseStream;
            baseStreamDataOffset = info.Offset;

            Name = info.Name;
            Size = info.Size;
            LastWriteTime = info.LastWriteTime;
            Unknown = info.Unknown;
        }

        internal override Stream GetStream()
        {
            baseStream.Position = baseStreamDataOffset;
            return new SubStream(baseStream, 0, Size, true);
        }
    }
}