// Decompiled with JetBrains decompiler
// Type: Database.UpgradeAllBasicBuildings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.IO;

namespace Database
{
  public class UpgradeAllBasicBuildings : ColonyAchievementRequirement
  {
    private Tag basicBuilding;
    private Tag upgradeBuilding;

    public UpgradeAllBasicBuildings(Tag basicBuilding, Tag upgradeBuilding)
    {
      this.basicBuilding = basicBuilding;
      this.upgradeBuilding = upgradeBuilding;
    }

    public override bool Success()
    {
      bool flag = false;
      foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
      {
        KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
        if (component.HasTag(this.basicBuilding))
          return false;
        if (component.HasTag(this.upgradeBuilding))
          flag = true;
      }
      return flag;
    }

    public override void Deserialize(IReader reader)
    {
      this.basicBuilding = new Tag(reader.ReadKleiString());
      this.upgradeBuilding = new Tag(reader.ReadKleiString());
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.WriteKleiString(this.basicBuilding.ToString());
      writer.WriteKleiString(this.upgradeBuilding.ToString());
    }

    public override string GetProgress(bool complete)
    {
      BuildingDef buildingDef1 = Assets.GetBuildingDef(this.basicBuilding.Name);
      BuildingDef buildingDef2 = Assets.GetBuildingDef(this.upgradeBuilding.Name);
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.UPGRADE_ALL_BUILDINGS, (object) buildingDef1.Name, (object) buildingDef2.Name);
    }
  }
}
