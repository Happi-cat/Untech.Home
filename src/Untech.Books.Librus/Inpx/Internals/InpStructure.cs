using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Books.Librus.Inpx.Internals
{
	internal class InpStructure
	{
		private const string DefaultStructure = "AUTHOR;GENRE;TITLE;SERIES;SERNO;FILE;SIZE;LIBID;DEL;EXT;DATE;";

		private InpStructure(IEnumerable<string> splittedStructure)
		{
			var structure = splittedStructure.ToList();

			Author = structure.IndexOf("AUTHOR");
			Genre = structure.IndexOf("GENRE");
			Title = structure.IndexOf("TITLE");
			Series = structure.IndexOf("SERIES");
			SeriesNo = structure.IndexOf("SERIESNO");
			File = structure.IndexOf("FILE");
			Size = structure.IndexOf("SIZE");
			LibId = structure.IndexOf("LIBID");
			Del = structure.IndexOf("DEL");
			Ext = structure.IndexOf("EXT");
			Date = structure.IndexOf("DATE");
			Language = structure.IndexOf("LANGUAGE");
			Keywords = structure.IndexOf("KEYWORDS");
			Folder = structure.IndexOf("FOLDER");
		}

		[MandatoryInpField] public int Author { get; }

		[MandatoryInpField] public int Genre { get; }

		[MandatoryInpField] public int Title { get; }

		[MandatoryInpField] public int Series { get; }

		[MandatoryInpField] public int SeriesNo { get; }

		[MandatoryInpField] public int File { get; }

		[MandatoryInpField] public int Size { get; }

		[MandatoryInpField] public int LibId { get; }

		[MandatoryInpField] public int Del { get; }

		[MandatoryInpField] public int Ext { get; }

		[MandatoryInpField] public int Date { get; }

		public int Language { get; }

		public int Keywords { get; }

		public int Folder { get; }

		public static InpStructure CreateDefault()
		{
			return Parse(DefaultStructure);
		}

		public static InpStructure Parse(string rawStructure)
		{
			if (string.IsNullOrEmpty(rawStructure)) throw new ArgumentNullException(nameof(rawStructure));

			var delimitedStructure = rawStructure.Split(';');
			return new InpStructure(delimitedStructure);
		}
	}
}