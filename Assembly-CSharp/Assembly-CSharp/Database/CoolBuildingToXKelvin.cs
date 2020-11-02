// Decompiled with JetBrains decompiler
// Type: Database.CoolBuildingToXKelvin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class CoolBuildingToXKelvin : ColonyAchievementRequirement
  {
    private int kelvinToCoolTo;

    public CoolBuildingToXKelvin(int kelvinToCoolTo) => this.kelvinToCoolTo = kelvinToCoolTo;

    public override bool Success() => (double) BuildingComplete.MinKelvinSeen <= (double) this.kelvinToCoolTo;

    public override void Deserialize(IReader reader) => this.kelvinToCoolTo = reader.ReadInt32();

    public override void Serialize(BinaryWriter writer) => writer.Write(this.kelvinToCoolTo);

    public override string GetProgress(bool complete)
    {
      float minKelvinSeen = BuildingComplete.MinKelvinSeen;
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, (object) minKelvinSeen);
    }
  }
}
