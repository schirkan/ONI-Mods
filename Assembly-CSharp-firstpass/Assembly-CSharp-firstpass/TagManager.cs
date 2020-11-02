// Decompiled with JetBrains decompiler
// Type: TagManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public class TagManager
{
  private static Dictionary<Tag, string> ProperNames = new Dictionary<Tag, string>();
  private static Dictionary<Tag, string> ProperNamesNoLinks = new Dictionary<Tag, string>();
  public static readonly Tag Invalid = new Tag();

  public static Tag Create(string tag_string)
  {
    Tag key = new Tag();
    key.Name = tag_string;
    if (!TagManager.ProperNames.ContainsKey(key))
    {
      TagManager.ProperNames[key] = "";
      TagManager.ProperNamesNoLinks[key] = "";
    }
    return key;
  }

  public static Tag Create(string tag_string, string proper_name)
  {
    Tag key = TagManager.Create(tag_string);
    if (string.IsNullOrEmpty(proper_name))
      DebugUtil.Assert(false, "Attempting to set proper name for tag: " + tag_string + "to null or empty.");
    TagManager.ProperNames[key] = proper_name;
    TagManager.ProperNamesNoLinks[key] = TagManager.StripLinkFormatting(proper_name);
    return key;
  }

  public static Tag[] Create(IList<string> strings)
  {
    Tag[] tagArray = new Tag[strings.Count];
    for (int index = 0; index < strings.Count; ++index)
      tagArray[index] = TagManager.Create(strings[index]);
    return tagArray;
  }

  public static void FillMissingProperNames()
  {
    foreach (Tag key in new List<Tag>((IEnumerable<Tag>) TagManager.ProperNames.Keys))
    {
      if (string.IsNullOrEmpty(TagManager.ProperNames[key]))
      {
        TagManager.ProperNames[key] = TagDescriptions.GetDescription(key.Name);
        TagManager.ProperNamesNoLinks[key] = TagManager.StripLinkFormatting(TagManager.ProperNames[key]);
      }
    }
  }

  public static string GetProperName(Tag tag, bool stripLink = false)
  {
    string str = (string) null;
    return stripLink && TagManager.ProperNamesNoLinks.TryGetValue(tag, out str) || !stripLink && TagManager.ProperNames.TryGetValue(tag, out str) ? str : tag.Name;
  }

  public static string StripLinkFormatting(string text)
  {
    string str = text;
    try
    {
      while (str.Contains("<link="))
      {
        int startIndex1 = str.IndexOf("</link>");
        if (startIndex1 > -1)
          str = str.Remove(startIndex1, 7);
        else
          Debug.LogWarningFormat("String has no closing link tag: {0}", (object[]) Array.Empty<object>());
        int startIndex2 = str.IndexOf("<link=");
        if (startIndex2 != -1)
          str = str.Remove(startIndex2, 7);
        else
          Debug.LogWarningFormat("String has no open link tag: {0}", (object[]) Array.Empty<object>());
        int num = str.IndexOf("\">");
        if (num != -1)
          str = str.Remove(startIndex2, num - startIndex2 + 2);
        else
          Debug.LogWarningFormat("String has no open link tag: {0}", (object[]) Array.Empty<object>());
      }
    }
    catch
    {
      Debug.Log((object) ("STRIP LINK FORMATTING FAILED ON: " + text));
      str = text;
    }
    return str;
  }
}
