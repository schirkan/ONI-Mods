// Decompiled with JetBrains decompiler
// Type: NavOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public struct NavOffset
{
  public NavType navType;
  public CellOffset offset;

  public NavOffset(NavType nav_type, int x, int y)
  {
    this.navType = nav_type;
    this.offset.x = x;
    this.offset.y = y;
  }
}
