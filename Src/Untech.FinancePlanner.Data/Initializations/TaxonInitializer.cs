using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using YamlDotNet.RepresentationModel;

namespace Untech.FinancePlanner.Data.Initializations
{
	public static class TaxonInitializer
	{
		public static void Initialize(FinancialPlannerContext context, string directory)
		{
			context.Database.EnsureCreated();

			if (context.Taxons.Any())
			{
				return;
			}

			Initialize(context, BuiltInTaxonId.Expense, Path.Combine(directory, "Expenses.eyaml"));
			Initialize(context, BuiltInTaxonId.Saving, Path.Combine(directory, "Savings.eyaml"));
			Initialize(context, BuiltInTaxonId.Income, Path.Combine(directory, "Incomes.eyaml"));
		}

		private static void Initialize(FinancialPlannerContext context, int rootId, string filePath)
		{
			var yaml = new YamlStream();
			using (var stream = File.OpenRead(filePath))
			{
				yaml.Load(new StreamReader(stream));

				var visitor = new TreeVisitor();
				visitor.Visit(yaml);

				Initialize(context, rootId, visitor.GetResult().Elements);
			}
		}

		private static void Initialize(FinancialPlannerContext context, int parentId, IEnumerable<Tree> elements)
		{
			foreach (var element in elements)
			{
				var entry = context.Taxons.Add(new Taxon(0, parentId, element.Name));
				context.SaveChanges();
				Initialize(context, entry.Entity.Key, element.Elements);
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