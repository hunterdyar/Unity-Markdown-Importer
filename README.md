# Unity Markdown Importer
Parse Frontmatter data from Markdown files in Unity. This custom importer creates a "Markdown Object" asset, which has a GetFrontmatter<T>() function for reading the data. The body text is left in a TextAsset to be read and used as normal.

## How To Use

### 1. Create Frontmatter Type

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

### 2. Select Importer

Next, select the text asset for your markdown file (".md" or ".markdown" extensions recognized by default). ".md" files are handled as TextAssets by default, so change the "importer" dropdown to use the markdown importer.
![Selecting import at top of asset import options](Documentation/importerDropdown.png)

### 3. Select Frontmatter Type 

All classes that extend from IFrontmatter will be listed in the dropdown. Choose the appropriate one (you probably need to create this) and click "Apply". Look at the imported object settings to see if it has serialized correctly.

![Import Settings](Documentation\import.png)

## Limitations

Frontmatter MUST use three hyphens on the first line, and at the end of the frontmatter. This is standard. 

The biggest limitation is the YAMLParser. I quickly put together a simple one, that can only handle lines of  "key: value". It breaks easily, doesn't support arrays or subobjects, and isn't even close to full "YAML" spec. Unity saves scene files in it's own flavor of YAML, so it feels silly - I'm reluctant to actually dig in and write my own - but this parser doesn't appear to be exposed. 

> One hacky workaround would be to create a prefab asset and inject the parseing yaml into it this pretend file, then read the frontmatter front that after Unity's internal systems parse it. This also feels silly.
>
> Another less hacky workaround is to use [YAMLDotNet](https://github.com/aaubry/YamlDotNet) or similar other tool. I don't need full YAML specifications for the project I made this for, so I am not bothering.

In many versions of Markdown, you can use a JSON object for frontmatter. This is not supported.
