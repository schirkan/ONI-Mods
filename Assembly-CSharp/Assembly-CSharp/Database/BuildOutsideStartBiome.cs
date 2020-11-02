﻿// Decompiled with JetBrains decompiler
// Type: Database.BuildOutsideStartBiome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using Klei;
using ProcGen;
using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class BuildOutsideStartBiome : ColonyAchievementRequirement
  {
    public override bool Success()
    {
      WorldDetailSave worldDetailSave = SaveLoader.Instance.worldDetailSave;
      for (int index = 0; index < worldDetailSave.overworldCells.Count; ++index)
      {
        WorldDetailSave.OverworldCell overworldCell = worldDetailSave.overworldCells[index];
        if (overworldCell.tags != null && !overworldCell.tags.Contains(WorldGenTags.StartWorld))
        {
          Polygon poly = overworldCell.poly;
          foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
          {
            if (!buildingComplete.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding) && poly.PointInPolygon((Vector2) buildingComplete.transform.GetPosition()))
            {
              Game.Instance.unlocks.Unlock("buildoutsidestartingbiome");
              return true;
            }
          }
        }
      }
      return false;
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override string GetProgress(bool complete) => (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_OUTSIDE_START;
  }
}
