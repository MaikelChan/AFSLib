# AFSLib

![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/MaikelChan/AFSLib?sort=semver&style=for-the-badge)
![Nuget](https://img.shields.io/nuget/v/MaikelChan.AFSLib?color=blueviolet&style=for-the-badge)
![GitHub commits since latest release (by SemVer)](https://img.shields.io/github/commits-since/MaikelChan/AFSLib/latest?color=orange&sort=semver&style=for-the-badge)
![GitHub](https://img.shields.io/github/license/MaikelChan/AFSLib?style=for-the-badge)

AFSLib is a library that can extract, create and manipulate AFS files. The AFS format is used in many games from companies like Sega. Even though it's a simple format, there are lots of quirks and edge cases in many games that require implementing specific fixes or workarounds for the library to work with them. If you encounter any issue with a specific game, don't hesitate to report it.

For a usage example, you can check [AFSPacker](https://github.com/MaikelChan/AFSPacker), a simple command line tool that uses AFSLib to extract and create AFS files.

## Usage examples

In order to use this examples, make sure you are using AFSLib.

```cs
using AFSLib;
```

### Create an AFS archive from scratch with some files

```cs
// Create a new AFS object.

using (AFS afs = new AFS())
{
    // Add two files. The first parameter is the path to the file.
    // By default, an entry will be created with the same name as the file.
    // But you can also set a different entry name with the second parameter.

    afs.AddEntryFromFile(@"C:\Data\Files\MyTextFile.txt");
    afs.AddEntryFromFile(@"C:\Data\Files\MyImageFile.png", "Title.png");

    // Finally save the AFS archive to the specified file.

    afs.SaveToFile(@"C:\Data\MyArchive.afs");
}
```

### Create an AFS archive out of all the files inside a directory

```cs
// Create a new AFS object.

using (AFS afs = new AFS())
{
    // Add all the files located in the directory C:\Data\Files.

    afs.AddEntriesFromDirectory(@"C:\Data\Files");

    // Finally save the AFS archive to the specified file.

    afs.SaveToFile(@"C:\Data\MyArchive.afs");
}
```

### Extract all the entries from an existing AFS archive

```cs
// Create an AFS object out of an existing AFS archive.

using (AFS afs = new AFS(@"C:\Data\MyArchive.afs"))
{
    // Extract all the entries to the specified directory.

    afs.ExtractAllEntries(@"C:\Data\Files");
}
```

### Change some entries properties and save them in another AFS archive

```cs
// Create an AFS object out of an existing AFS archive.

using (AFS afs = new AFS(@"C:\Data\MyArchive.afs"))
{
    // Change some properties.

    afs.Entries[0].Name = "NewFileName.txt";
    afs.Entries[0].LastWriteTime = DateTime.Now;

    // Save again with a different name.

    afs.SaveToFile(@"C:\Data\MyNewArchive.afs");
}
```

### Advanced AFS creation

There are many variants of the AFS format found in many games. Some of those games may require to have a specific variant of AFS archive. There are several modifiable properties that allow to customize the archive so it matches what the game needs. To know what values are needed, read them from one of the AFS archives found in the game.

```cs
// Some variables to store the properties

HeaderMagicType header;
AttributesInfoType attributesInfo;
uint entryBlockAlignment;

// Create an AFS object out of an existing AFS archive.

using (AFS afs = new AFS(@"C:\Data\MyArchive.afs"))
{
    // Read and store some properties

    header = afs.HeaderMagicType;
    attributesInfo = afs.AttributesInfoType;
    entryBlockAlignment = afs.EntryBlockAlignment;
}

// Create a new AFS object.

using (AFS afs = new AFS())
{
    // Set the same properties to the new file

    afs.HeaderMagicType = header;
    afs.AttributesInfoType = attributesInfo;
    afs.EntryBlockAlignment = entryBlockAlignment;

    // Add some files

    afs.AddEntriesFromDirectory(@"C:\Data\Files");

    // Finally save the AFS archive to the specified file.

    afs.SaveToFile(@"C:\Data\MyArchive.afs");
}
```

### Get and print progress information

```cs
// Create an AFS object out of an existing AFS archive.

using (AFS afs = new AFS(@"C:\Data\MyArchive.afs"))
{
    // Subscribe to the event so the extraction process will send info about its progress
    // and can be printed with the method Afs_NotifyProgress.

    afs.NotifyProgress += Afs_NotifyProgress;

    // Extract all the entries to the specified directory.

    afs.ExtractAllEntries(@"C:\Data\Files");
}

static void Afs_NotifyProgress(NotificationType type, string message)
{
    Console.WriteLine($"[{type}] - {message}");
}
```