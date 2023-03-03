# Unity Markdown Importer
Parse Frontmatter data from Markdown files in Unity. This custom importer creates a "Markdown Object" asset, which has a GetFrontmatter<T>() function for reading the data. The body text is left in a TextAsset to be read and used as normal.
  
## How To Use
Create a class that is [Serializable] and implements IFrontmatter, which is an empty interface for convenience. The class should have public fields that match the names of the frontmatter:
  
  ```
  [Serializable]
	public class BlogPostFrontmatter : IFrontmatter
	{
		public string title;
		public bool draft;
    public int weight;
	}
  ```
  
  
  
