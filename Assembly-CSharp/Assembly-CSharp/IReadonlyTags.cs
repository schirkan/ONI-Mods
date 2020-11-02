// Decompiled with JetBrains decompiler
// Type: IReadonlyTags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IReadonlyTags
{
  bool HasTag(string tag);

  bool HasTag(int hashtag);

  bool HasTags(int[] tags);
}
