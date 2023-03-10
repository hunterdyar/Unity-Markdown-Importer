using System;
using System.Collections.Generic;

namespace HDyar.MarkdownImporter.YAMLUtility
{
	public static class SimpleFrontmatterParse
	{
		public static T FromYAML<T>(string data) where T : new()
		{
			var values = new T();
			//split the data up by lines, split each line up by : to make elements.
			//work through the symbols and identify keys and values, and create a list of keyvalues
			//for each piece of parsed data, search T for a member that has the same name as the key
			//attempt to parse value into the type of member.
			if (string.IsNullOrEmpty(data))
			{
				//empty. return empty.
				return values;
			}
			List<YAMLKeyValue> matter = new List<YAMLKeyValue>();
			var lines = data.Split('\n');
			foreach (var line in lines)
			{
				//Strip Comments
				string l = line;
				int comment = line.IndexOf('#');
				if (comment != -1)
				{
					l = line.Substring(0, line.IndexOf('#'));
				}
				//Find keys and values
				var elements = l.Split(':');
				if (elements is { Length: 2 })
				{
					matter.Add(new YAMLKeyValue(elements[0],elements[1]));
				}
				else
				{
					//i need a real YAML parser :(
					//I think i can do that, but first lets figure out how the reflection works.
					//it's so silly to write a YAML parser because unity has UnityYAML built into it, its seemingly just not exposed.
					//There's almost certainly a way to hack ourselves a yaml parser by serializing an asset then reading the file directly
					//which is equally absurd.
				}
				
				///
				var type = typeof(T);
				var properties = type.GetFields();
				foreach (var item in matter)
				{
					foreach (var field in properties)
					{
						if (field.Name.ToLower() == item.key.ToLower())
						{
							//switch?
							if (field.FieldType == typeof(string))
							{
								string noQuotes = (item.value[0] == '"' && item.value[^1] == '"') ? item.value.Substring(1, item.value.Length - 2) : item.value;
								field.SetValue(values, noQuotes);
							}else if (field.FieldType == typeof(int))
							{
								if (int.TryParse(item.value, out int val))
								{
									field.SetValue(values, val);
								}
							}else if (field.FieldType == typeof(float))
							{
								if (float.TryParse(item.value, out var val))
								{
									field.SetValue(values, val);
								}
							}else if (field.FieldType == typeof(char))
							{
								if (char.TryParse(item.value, out var val))
								{
									field.SetValue(values, val);
								}
							}else if (field.FieldType == typeof(double))
							{
								if (double.TryParse(item.value, out var val))
								{
									field.SetValue(values, val);
								}
							}else if (field.FieldType == typeof(bool))
							{
								if (Boolean.TryParse(item.value, out var val))
								{
									field.SetValue(values, val);
								}
							}
							else if (field.FieldType == typeof(byte))
							{
								if (byte.TryParse(item.value, out byte val))
								{
									field.SetValue(values, val);
								}
							}
							//check if it an enum
				
							//sbyte
							//decimal
							//uint
							//nint
							//nuint
							//ulong
							//short
							//ushort
							//enums?
						}
					}
				}
			}
			return values;
		}
	}


	public class YAMLKeyValue
	{
		public string key;
		public string value;

		public YAMLKeyValue(string key, string val)
		{
			this.key = key.Trim();
			this.value = val.Trim();
		}
	}
}