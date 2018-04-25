using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Untech.Books.Librus.Inpx.Internals;
using Untech.Books.Librus.Models;

namespace Untech.Books.Librus.Inpx
{
	public class InpxReader : IDisposable
	{
		private static class EntryNames
		{
			public const string Structure = "structure.info";
			public const string Collection = "collection.info";
			public const string Version = "version.info";
		}

		private bool _disposed;
		private readonly ZipArchive _zipArchive;

		public InpxReader(Stream stream)
		{
			_zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
		}

		public CollectionInfo ReadInfo()
		{
			if (_disposed) throw new ObjectDisposedException("InpxReader was disposed");

			var info = new CollectionInfo();

			var collectionInfoEntry = _zipArchive.GetEntry(EntryNames.Collection);

			using (var stream = collectionInfoEntry.Open())
			using (var reader = new StreamReader(stream))
			{
				info.Name = reader.ReadLine();
				info.FileName = reader.ReadLine();
				info.Type = reader.ReadLine();
				if (!reader.EndOfStream)
				{
					info.Description = reader.ReadToEnd();
				}
			}

			var versionInfoEntry = _zipArchive.GetEntry(EntryNames.Version);
			using (var stream = versionInfoEntry.Open())
			using (var reader = new StreamReader(stream))
			{
				info.Version = reader.ReadLine();
			}

			return info;
		}

		public IEnumerable<BookInfo> ReadAll()
		{
			if (_disposed) throw new ObjectDisposedException("InpxReader was disposed");

			var structure = ReadStructure();
			var parser = new InpEntryParser(structure);
			parser.Validate();

			foreach (var inpEntry in _zipArchive.Entries.Where(n => n.Name.EndsWith(".inp")))
			{
				using (var stream = inpEntry.Open())
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						yield return parser.Parse(reader.ReadLine());
					}
				}
			}
		}

		private InpStructure ReadStructure()
		{
			var structureEntry = _zipArchive.Entries.SingleOrDefault(n => n.Name == EntryNames.Structure);
			if (structureEntry != null)
			{
				var entry = _zipArchive.GetEntry(EntryNames.Structure);
				using (var stream = entry.Open())
				using (var reader = new StreamReader(stream))
				{
					return InpStructure.Parse(reader.ReadLine());
				}
			}

			return InpStructure.CreateDefault();
		}

		public void Dispose()
		{
			if (_disposed) return;

			_zipArchive.Dispose();
			_disposed = true;
		}
	}
}