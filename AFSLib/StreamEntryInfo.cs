﻿using System;

namespace AFSLib
{
    internal struct StreamEntryInfo
    {
        public uint Offset;
        public string Name;
        public uint Size;
        public DateTime LastWriteTime;
        public uint CustomData;

        public bool IsNull => Offset == 0;
    }
}