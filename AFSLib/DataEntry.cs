﻿using System;

namespace AFSLib
{
    public abstract class DataEntry : Entry
    {
        /// <summary>
        /// The name of the entry, which can't be longer than 32 characters, including extension.
        /// It can contain special characters like ":", be an empty string,
        /// and it can be the same name as other entries.
        /// For that reason, it's not safe to use this name to extract a file into the operating system.
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (value.Length > AFS.MAX_ENTRY_NAME_LENGTH)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"Entry name can't be longer than {AFS.MAX_ENTRY_NAME_LENGTH} characters: \"{value}\".");
                }

                if (value == null)
                {
                    value = string.Empty;
                }

                name = value;
                NameChanged?.Invoke();
            }
        }
        private string name;

        /// <summary>
        /// An autogenerated name of the entry.
        /// It will be unique and won't contain special characters like ":".
        /// This can be safely used to name extracted files into the operating system.
        /// </summary>
        public string SanitizedName { get; internal set; }

        /// <summary>
        /// Size of the entry data.
        /// </summary>
        public uint Size { get; protected set; }

        /// <summary>
        /// Date of the last time the entry was modified.
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// In many AFS archives this contains the entry size, but some of them contain some unknown values.
        /// </summary>
        public uint Unknown { get; set; }

        internal event Action NameChanged;

        #region Deprecated

        /// <summary>
        /// Renames an entry.
        /// </summary>
        /// <param name="name">The new name to assign to the entry.</param>
        [Obsolete("This method is deprecated since version 1.1.0, please use the Name property.")]
        public void Rename(string name)
        {
            Name = name;
        }

        #endregion
    }
}