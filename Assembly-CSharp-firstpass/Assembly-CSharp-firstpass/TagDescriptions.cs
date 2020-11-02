// Decompiled with JetBrains decompiler
// Type: TagDescriptions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Text;

public class TagDescriptions
{
  public TagDescriptions(string csv_data)
  {
  }

  public static string GetDescription(string tag) => (string) Strings.Get("STRINGS.MISC.TAGS." + tag.ToUpper());

  public static string GetDescription(Tag tag) => (string) Strings.Get("STRINGS.MISC.TAGS." + tag.Name.ToUpper());

  public static string ReplaceTags(string text)
  {
    int startIndex1 = text.IndexOf('{');
    int num = text.IndexOf('}');
    if (0 > startIndex1 || startIndex1 >= num)
      return text;
    StringBuilder stringBuilder = new StringBuilder();
    int startIndex2 = 0;
    int startIndex3;
    for (; 0 <= startIndex1; startIndex1 = text.IndexOf('{', startIndex3))
    {
      string str = text.Substring(startIndex2, startIndex1 - startIndex2);
      stringBuilder.Append(str);
      startIndex3 = text.IndexOf('}', startIndex1);
      if (startIndex1 < startIndex3)
      {
        string description = TagDescriptions.GetDescription(text.Substring(startIndex1 + 1, startIndex3 - startIndex1 - 1));
        stringBuilder.Append(description);
        startIndex2 = startIndex3 + 1;
      }
      else
        break;
    }
    stringBuilder.Append(text.Substring(startIndex2, text.Length - startIndex2));
    return stringBuilder.ToString();
  }
}
