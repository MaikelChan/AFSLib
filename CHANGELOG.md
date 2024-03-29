# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.3] - 2023-09-11
### Added
- Added `AllAttributesContainEntrySize` property easily check if all the attributes of an AFS archive contain the entry size or a customized data. 
### Fixed
- Fixed interpreting 0 byte files as Null entries, which broke games like Urban Reign.
### Deprecated
- Deprecated the `UnknownAttribute` and related properties. It is now called `CustomData`.

## [2.0.2] - 2021-10-26
### Fixed
- Fixed regression: now it's able to handle AFS files with invalid dates.

## [2.0.1] - 2021-09-06
### Fixed
- Fixed missing XML documentation in NuGet package.

## [2.0.0] - 2021-09-06
### Added
- Entry block alignment is now configurable.
- AFS constructor now accepts a file path.
- Added method to add all files in a directory and, optionally, its subdirectories.
- Added a method to save directly to a file.
- AddEntry* methods now return a reference to the added entry.
- Implemented null entries. They are created with NullEntry, and fixed all issues related to not being able to remove them or add them.

### Changed
- AFS class is now IDisposable. Dispose needs to be called to close the internal FileStream in case the AFS object is created from a file.
- When adding an entry, null entry names are considered as string.Empty.
- Improved XML documentation.
- Renamed Name to SanitizedName and RawName to Name. This makes more sense because Name is the actual name of an entry, and SanitizedName is the autogenerated read-only name that contains no invalid characters.
- UnknownAttribute and LastWriteTime properties are now modifiable.

### Deprecated
- Deprecated AddEntry, ExtractEntry and ExtractAllEntries. Now they are called AddEntryFromFile, AddEntryFromStream, ExtractEntryToFile, ExtractAllEntriesToDirectory.
- In an entry, Rename has been deprecated. Use the Name property instead.
- In an entry, Unknown property has been deprecated. Use the UnknownAttribute property instead.

### Fixed
- Make sure an entry name cannot be longer than 32 characters.

## [1.0.1] - 2021-09-01
### Added
- Be able to add an entry from a Stream to an AFS file.
### Fixed
- Fixed an exception message.

## [1.0.0] - 2021-09-01
- Initial release.