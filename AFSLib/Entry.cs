using System.IO;

namespace AFSLib
{
    public abstract class Entry
    {
        internal abstract Stream GetStream();
    }
}