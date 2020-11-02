// Decompiled with JetBrains decompiler
// Type: ProcGen.Mob
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;
using System;

namespace ProcGen
{
  [Serializable]
  public class Mob : SampleDescriber
  {
    public Mob()
    {
    }

    public Mob(Mob.Location location) => this.location = location;

    public MinMax units { get; private set; }

    public string prefabName { get; private set; }

    public int width { get; private set; }

    public int height { get; private set; }

    [StringEnumConverter]
    public Mob.Location location { get; private set; }

    public enum Location
    {
      Floor,
      Ceiling,
      Air,
      BackWall,
      NearWater,
      NearLiquid,
      Solid,
      Water,
      ShallowLiquid,
      Surface,
      LiquidFloor,
      AnyFloor,
    }
  }
}
