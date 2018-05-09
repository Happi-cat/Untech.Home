using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Untech.Books.Librus.Models;

namespace Untech.Books.Librus.Inpx.Internals
{
	internal class InpEntryParser
	{
		private const char FieldDelimiter = '\x04';
		private const char FieldPartsDelimiter = ':';
		private const char FieldPartsItemsDelimiter = ',';


		private readonly InpStructure _structure;

		public InpEntryParser(InpStructure structure)
		{
			_structure = structure;
		}

		public string InpName { get; set; }

		public void Validate()
		{
			Validator.ValidateObject(_structure, new ValidationContext(_structure));

			if (_structure.Folder == -1 && string.IsNullOrEmpty(InpName))
			{
				throw new ValidationException("Folder field is missing so InpName should be specified");
			}
		}

		public BookInfo Parse(string raw)
		{
			if (string.IsNullOrEmpty(raw)) throw new ArgumentNullException(nameof(raw));

			var fields = raw.Split(FieldDelimiter);

			return new BookInfo
			{
				Authors = ParseAuthors(fields[_structure.Author]),
				Genres = ParseGenre(fields[_structure.Genre]),
				Title = fields[_structure.Title],
				Series = SanitizeName(fields[_structure.Series]),
				SeriesNo = ParseInt(fields[_structure.SeriesNo]),
				File = fields[_structure.File],
				Ext = fields[_structure.Ext],
				Date = DateTime.Parse(fields[_structure.Date]),
				Language = _structure.Language != -1 ? fields[_structure.Language] : null,
				Archive = ParseArchive(fields),
				Meta = ParseMetaFields(fields)
			};
		}

		private string ParseArchive(IReadOnlyList<string> fields)
		{
			if (_structure.Folder > -1)
			{
				return fields[_structure.Folder];
			}

			if (string.IsNullOrEmpty(InpName)) throw new InvalidOperationException("Folder field is missing so InpName should be specified");

			return InpName.Replace(".inp", ".zip");
		}

		private IReadOnlyList<string> ParseGenre(string rawGenre)
		{
			return rawGenre.Split(new[] {FieldPartsDelimiter}, StringSplitOptions.RemoveEmptyEntries);
		}

		private IReadOnlyList<MetaField> ParseMetaFields(IReadOnlyList<string> fields)
		{
			var meta = new List<MetaField>
			{
				new MetaField
				{
					Name = "size",
					Value = fields[_structure.Size]
				},
				new MetaField
				{
					Name = "libraryid",
					Value = fields[_structure.LibId]
				},
				new MetaField
				{
					Name = "deleted",
					Value = (fields[_structure.Del] == "1").ToString()
				},
			};

			if (_structure.Keywords > -1)
			{
				meta.AddRange(fields[_structure.Keywords]
					.Split(new[] {FieldPartsDelimiter}, StringSplitOptions.RemoveEmptyEntries)
					.Select(keyword => new MetaField
					{
						Name = "keyword",
						Value = keyword
					}));
			}

			return meta;
		}

		private List<Author> ParseAuthors(string rawAuthors)
		{
			var splittedRawAuthors = rawAuthors.Split(new[] {FieldPartsDelimiter}, StringSplitOptions.RemoveEmptyEntries);

			return splittedRawAuthors
				.Select(rawAuthor => rawAuthor.Split(FieldPartsItemsDelimiter))
				.Select(authorNames => new Author
				{
					LastName = SanitizeName(authorNames[0]),
					FirstName = SanitizeName(authorNames.ElementAtOrDefault(1)),
					MiddleName = SanitizeName(authorNames.ElementAtOrDefault(2)),
				})
				.ToList();
		}

		private string SanitizeName(string name)
		{
			if (name == null) return null;
			name = name.Trim();
			return name == "" || name == "--" ? null : name;
		}

		private int ParseInt(string s)
		{
			return string.IsNullOrWhiteSpace(s) ? -1 : int.Parse(s);
		}
	}
}