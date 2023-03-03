using System;

namespace HDyar.MarkdownImporter
{
	[Serializable]
	public class EmptyFrontmatterType : IFrontmatter
	{
		//Having some class frontmatter type prevents the editor from bugging out - which it will when there are no types of "IFrontmatter" 
	}
}