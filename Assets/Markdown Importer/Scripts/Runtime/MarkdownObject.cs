using System;
using System.Reflection;
using HDyar.MarkdownImporter.YAMLUtility;
using UnityEngine;

namespace HDyar.MarkdownImporter
{
	public class MarkdownObject : ScriptableObject
	{
		//Currently there is no advantage to using this importer.
		//but... frontmatter support into objects!

		[SerializeReference]
		public object Frontmatter;
		
		[TextArea(10,20)]
		public string FrontmatterText;

		public TextAsset Body;
		public void ParseFrontmatter(Type frontType, string front)
		{
			FrontmatterText = front;
			if (!string.IsNullOrEmpty(front))
			{
				//lol this is so gross. we're just throwing type safety out the window.
				//but like... yeah. Kind of the point?
				MethodInfo method = typeof(SimpleFrontmatterParse).GetMethod("FromYAML");
				method = method.MakeGenericMethod(new Type[] { frontType });

				Frontmatter = method?.Invoke(this, new object[] { front });
			}
		}

		public void Init(Type frontType, string front, TextAsset body)
		{
			ParseFrontmatter(frontType,front);
			this.Body = body;
		}

		public T GetFrontmatter<T>() where T : IFrontmatter
		{
			return (T)Frontmatter;
		}
	}
}