

using System;
using HDyar.MarkdownImporter;

namespace ObsidianCanvas.Frontmatter
{
	[Serializable]
	public class BlogPostFrontmatter : IFrontmatter
	{
		public string title;
		public bool draft;
		public string date;//we will have to manually parse strings into DateTimes, or whatever one might do, since "date" isn't a supported type in my basic YAML parser.
	}
}