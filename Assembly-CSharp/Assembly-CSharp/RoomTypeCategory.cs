// Decompiled with JetBrains decompiler
// Type: RoomTypeCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class RoomTypeCategory : Resource
{
  public string colorName { get; private set; }

  public RoomTypeCategory(string id, string name, string colorName)
    : base(id, name)
    => this.colorName = colorName;
}
