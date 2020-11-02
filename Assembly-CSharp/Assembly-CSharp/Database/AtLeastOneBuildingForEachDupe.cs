// Decompiled with JetBrains decompiler
// Type: Database.AtLeastOneBuildingForEachDupe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class AtLeastOneBuildingForEachDupe : ColonyAchievementRequirement
  {
    private List<Tag> validBuildingTypes = new List<Tag>();

    public AtLeastOneBuildingForEachDupe(List<Tag> validBuildingTypes) => this.validBuildingTypes = validBuildingTypes;

    public override bool Success()
    {
      if (Components.LiveMinionIdentities.Items.Count <= 0)
        return false;
      int num = 0;
      foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
      {
        Tag prefabTag = basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag;
        if (this.validBuildingTypes.Contains(prefabTag))
        {
          ++num;
          if (prefabTag == (Tag) "FlushToilet" || prefabTag == (Tag) "Outhouse")
            return true;
        }
      }
      return num >= Components.LiveMinionIdentities.Items.Count;
    }

    public override bool Fail() => false;

    public override void Deserialize(IReader reader)
    {
      int capacity = reader.ReadInt32();
      this.validBuildingTypes = new List<Tag>(capacity);
      for (int index = 0; index < capacity; ++index)
        this.validBuildingTypes.Add(new Tag(reader.ReadKleiString()));
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.validBuildingTypes.Count);
      foreach (Tag validBuildingType in this.validBuildingTypes)
        writer.WriteKleiString(validBuildingType.ToString());
    }

    public override string GetProgress(bool complete)
    {
      if (this.validBuildingTypes.Contains((Tag) "FlushToilet"))
        return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_TOILET;
      if (complete)
        return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_BED_PER_DUPLICANT;
      int num = 0;
      foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
      {
        if (this.validBuildingTypes.Contains(basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag))
          ++num;
      }
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILING_BEDS, (object) (complete ? Components.LiveMinionIdentities.Items.Count : num), (object) Components.LiveMinionIdentities.Items.Count);
    }
  }
}
