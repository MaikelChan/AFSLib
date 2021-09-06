using System.IO;

namespace AFSLib
{
    public class NullEntry : Entry
    {
        internal NullEntry()
        {

        }

        internal override Stream GetStream()
        {
            return null;
        }
    }
}