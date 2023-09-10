using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.Cryptography;

namespace AFSLib.Tests
{
    [TestClass]
    public class AFSLibTests
    {
        readonly string TestFilesDirectory = @"..\..\..\..\..\..\Test Files";

        readonly string[] TestFiles = new string[]
        {
            //"0_sound.afs",    // Pro Evolution Soccer 5 (Fails due to having random data between files. Rest is ok)
            //"ADX_USA.AFS",    // Dragon Ball Z Budokai 3 (Fails due to having an uint indicating the size of the whole AFS file. No other AFS has this. Rest is ok)
            "AREA50.AFS",       // Winback 2: Project Poseidon
            "btl_voice.afs",    // Arc Rise Fantasia
            //"DVD000.AFS",     // Urban Reign (PS2) (Fails due to adding missind dates)
            "movie.afs",        // Soul Calibur 2 (PS2)
            "mry.afs"           // Resident Evil: Code Veronica (GameCube)
        };

        [TestMethod]
        public void Regeneration()
        {
            using (MD5 md5 = MD5.Create())
            {
                for (int tf = 0; tf < TestFiles.Length; tf++)
                {
                    string filePath1 = Path.Combine(TestFilesDirectory, TestFiles[tf]);
                    string filePath2 = filePath1 + "_2";
                    string extractionDirectory = Path.ChangeExtension(filePath1, null);

                    byte[] hash1;
                    byte[] hash2;

                    HeaderMagicType header;
                    AttributesInfoType attributesInfo;
                    uint entryBlockAlignment;
                    string[] entriesNames;
                    string[] entriesSanitizedNames;
                    uint[] entriesUnknownAttributes;

                    using (FileStream stream1 = File.OpenRead(filePath1))
                    {
                        hash1 = md5.ComputeHash(stream1);
                    }

                    using (AFS afs1 = new AFS(filePath1))
                    {
                        header = afs1.HeaderMagicType;
                        attributesInfo = afs1.AttributesInfoType;
                        entryBlockAlignment = afs1.EntryBlockAlignment;
                        entriesNames = new string[afs1.Entries.Count];
                        entriesSanitizedNames = new string[afs1.Entries.Count];
                        entriesUnknownAttributes = new uint[afs1.Entries.Count];

                        for (int e = 0; e < entriesSanitizedNames.Length; e++)
                        {
                            if (afs1.Entries[e] is NullEntry)
                            {
                                entriesNames[e] = null;
                                entriesSanitizedNames[e] = null;
                                entriesUnknownAttributes[e] = 0;
                            }
                            else
                            {
                                DataEntry dataEntry = afs1.Entries[e] as DataEntry;

                                entriesNames[e] = dataEntry.Name;
                                entriesSanitizedNames[e] = dataEntry.SanitizedName;
                                entriesUnknownAttributes[e] = dataEntry.UnknownAttribute;
                            }
                        }

                        afs1.ExtractAllEntriesToDirectory(extractionDirectory);
                    }

                    using (AFS afs2 = new AFS())
                    {
                        afs2.HeaderMagicType = header;
                        afs2.AttributesInfoType = attributesInfo;
                        afs2.EntryBlockAlignment = entryBlockAlignment;

                        for (int e = 0; e < entriesSanitizedNames.Length; e++)
                        {
                            if (entriesSanitizedNames[e] == null && entriesNames[e] == null)
                            {
                                afs2.AddNullEntry();
                            }
                            else
                            {
                                FileEntry fileEntry = afs2.AddEntryFromFile(Path.Combine(extractionDirectory, entriesSanitizedNames[e]), entriesNames[e]);
                                fileEntry.UnknownAttribute = entriesUnknownAttributes[e];
                            }
                        }

                        afs2.SaveToFile(filePath2);
                    }

                    using (FileStream stream2 = File.OpenRead(filePath2))
                    {
                        hash2 = md5.ComputeHash(stream2);
                    }

                    Assert.IsTrue(CompareHashes(hash1, hash2), $"File \"{TestFiles[tf]}\" was not regenerated correctly.");

                    File.Delete(filePath2);
                    Directory.Delete(extractionDirectory, true);
                }
            }
        }

        bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length) return false;

            for (int h = 0; h < hash1.Length; h++)
            {
                if (hash1[h] != hash2[h]) return false;
            }

            return true;
        }
    }
}