// Decompiled with JetBrains decompiler
// Type: Delaunay.LR.SideHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Delaunay.LR
{
  public class SideHelper
  {
    public static Side Other(Side leftRight) => leftRight != Side.LEFT ? Side.LEFT : Side.RIGHT;
  }
}
