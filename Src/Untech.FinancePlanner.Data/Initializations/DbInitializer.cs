using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Untech.FinancePlanner.Data.Cache;
using Untech.FinancePlanner.Domain.Models;
using Untech.Home;
using YamlDotNet.RepresentationModel;

namespace Untech.FinancePlanner.Data.Initializations
{
	public static class DbInitializer
	{
		public static void Initialize(Func<FinancialPlannerContext> contextFactory, string directory)
		{
			using (var context = contextFactory())
			{
				if (context.Taxons.Any())
				{
					return;
				}
			}

			var taxonStorage = new TaxonStorage(contextFactory);
			Initialize(taxonStorage, BuiltInTaxonId.Expense, Path.Combine(directory, "Expenses.eyaml"));
			Initialize(taxonStorage, BuiltInTaxonId.Saving, Path.Combine(directory, "Savings.eyaml"));
			Initialize(taxonStorage, BuiltInTaxonId.Income, Path.Combine(directory, "Incomes.eyaml"));
		}

		private static void Initialize(TaxonStorage storage, int rootId, string filePath)
		{
			var yaml = new YamlStream();
			using (var stream = File.OpenRead(filePath))
			{
				yaml.Load(new StreamReader(stream));

				var visitor = new TreeVisitor();
				visitor.Visit(yaml);

				Initialize(storage, rootId, visitor.GetResult().Elements);
			}
		}

		private static void Initialize(TaxonStorage storage, int parentId, IEnumerable<Tree> elements)
		{
			foreach (var element in elements)
			{
				var entry = storage.Create(new Taxon(0, parentId, element.Name));
				Initialize(storage, entry.Key, element.Elements);
			}
		}

		[DebuggerDisplay("(Name = {Name})")]
		private class Tree
		{
			public string Name { get; set; }
			public List<Tree> Elements { get; } = new List<Tree>();
		}

		private class TreeVisitor : IYamlVisitor
		{
			private readonly Stack<Tree> _path;

			public TreeVisitor()
			{
				_path = new Stack<Tree>();
				_path.Push(new Tree());
			}

			public Tree GetResult()
			{
				return _path.Peek();
			}

			public void Visit(YamlStream stream)
			{
				foreach (var doc in stream.Documents)
				{
					doc.Accept(this);
				}
			}

			public void Visit(YamlDocument document)
			{
				document.RootNode.Accept(this);
			}

			public void Visit(YamlScalarNode scalar)
			{
				_path.Peek().Elements.Add(new Tree { Name = scalar.Value });
			}

			public void Visit(YamlSequenceNode sequence)
			{
				foreach (var node in sequence)
				{
					node.Accept(this);
				}
			}

			public void Visit(YamlMappingNode mapping)
			{
				foreach (var node in mapping.Children)
				{
					var nameNode = (YamlScalarNode)node.Key;

					var newTaxon = new Tree { Name = nameNode.Value };
					_path.Peek().Elements.Add(newTaxon);
					_path.Push(newTaxon);

					node.Value.Accept(this);

					_path.Pop();
				}
			}
		}
	}
}