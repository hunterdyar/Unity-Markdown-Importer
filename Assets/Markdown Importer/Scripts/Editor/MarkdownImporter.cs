using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace HDyar.MarkdownImporter
{
	[ScriptedImporter(1, new[] { "markdown" }, new[] { "md" })]
	public class MarkdownImporter : ScriptedImporter
	{
		private const string frontmatterDelim = "---";

		//todo: this needs to not be empty by default
		//but we don't know if there are any classes?
		//so throw errors correctly and skip frontmatter until assigned
		[SerializeField] private string selectedTypeName;

		public override void OnImportAsset(AssetImportContext ctx)
		{
			var markdownObject = ScriptableObject.CreateInstance<MarkdownObject>();
			var assetName = Path.GetFileNameWithoutExtension(ctx.assetPath);
			markdownObject.name = assetName + " obj";

			//parse the text file.

			var data = SeparateFrontmatter(File.ReadAllText(ctx.assetPath));
			//Create the frontmatter object
			Type markdownImportType;
			if (!string.IsNullOrEmpty(selectedTypeName))
			{
				markdownImportType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(p => typeof(IFrontmatter).IsAssignableFrom(p) && p.IsClass && p.Name == selectedTypeName);
			}
			else
			{
				//Nothing selected, like first import. just find the first one.
				markdownImportType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(p => typeof(IFrontmatter).IsAssignableFrom(p) && p.IsClass);
				selectedTypeName = markdownImportType.Name;
			}

			//create textAsset for just the body text.
			if (markdownImportType != null)
			{
				var bodyAsset = new TextAsset(data.Item2);
				markdownObject.Init(markdownImportType, data.Item1, bodyAsset);
				ctx.AddObjectToAsset(assetName + " body", bodyAsset);
				bodyAsset.name = assetName + " body";
				EditorUtility.SetDirty(markdownObject);
				ctx.AddObjectToAsset("Canvas Object", markdownObject);
				ctx.SetMainObject(markdownObject);
			}
		}

		public static (string, string) SeparateFrontmatter(string markdown)
		{
			if (string.IsNullOrEmpty(markdown))
			{
				return ("", "");
			}

			string[] lines = markdown.Split('\n');
			if (lines.Length < 3)
			{
				//too short to have frontmatter. you need two lines of frontmatter deliminaters + content
				return ("", markdown);
			}

			//No frontmatter. First line MUST be '---' for obsidian. WIndows uses "/r/n" and we split by "/n", so we need to trim. This will help with extra spaces after the --- too.
			if (lines[0].Trim() != frontmatterDelim)
			{
				return ("", markdown);
			}

			int separation = 0;
			//now we have to find the second deliminator.
			for (int i = 1; i < lines.Length; i++)
			{
				if (lines[i].Trim() == frontmatterDelim)
				{
					separation = i;
					break;
				}
			}

			if (separation == 0)
			{
				return ("", markdown);
			}

			//now lines between 0 and separation is frontmatter
			//and lines after sepearation is content.
			string frontmatter = string.Join("\n", lines, 1, separation - 1);
			string justMarkdown = string.Join("\n", lines, separation + 1, lines.Length - separation - 1);
			return (frontmatter, justMarkdown);
		}
	}
}