// Decompiled with JetBrains decompiler
// Type: KMod.FileSystemItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace KMod
{
  public struct FileSystemItem
  {
    public string name;
    public FileSystemItem.ItemType type;

    public enum ItemType
    {
      Directory,
      File,
    }
  }
}
