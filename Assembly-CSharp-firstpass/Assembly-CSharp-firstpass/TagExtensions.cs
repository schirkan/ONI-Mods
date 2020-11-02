// Decompiled with JetBrains decompiler
// Type: TagExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public static class TagExtensions
{
  public static Tag ToTag(this string str) => new Tag(str);

  public static Tag[] ToTagArray(this string[] strArray)
  {
    Tag[] tagArray = new Tag[strArray.Length];
    for (int index = 0; index < strArray.Length; ++index)
      tagArray[index] = strArray[index].ToTag();
    return tagArray;
  }

  public static List<Tag> ToTagList(this string[] strArray)
  {
    List<Tag> tagList = new List<Tag>();
    foreach (string str in strArray)
      tagList.Add(str.ToTag());
    return tagList;
  }

  public static List<Tag> ToTagList(this List<string> strList)
  {
    List<Tag> tagList = new List<Tag>();
    strList.ForEach((System.Action<string>) (str => tagList.Add(str.ToTag())));
    return tagList;
  }
}
