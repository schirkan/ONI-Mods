// Decompiled with JetBrains decompiler
// Type: MonumentTopConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MonumentTopConfig : IBuildingConfig
{
  public const string ID = "MonumentTop";

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[3]
    {
      2500f,
      2500f,
      5000f
    };
    string[] construction_materials = new string[3]
    {
      SimHashes.Glass.ToString(),
      SimHashes.Diamond.ToString(),
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues incomplete = BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MonumentTop", 5, 5, "victory_monument_upper_kanim", 1000, 60f, construction_mass, construction_materials, 9999f, BuildLocationRule.BuildingAttachPoint, incomplete, noise);
    BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.AttachmentSlotTag = (Tag) "MonumentTop";
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<MonumentPart>().part = MonumentPart.Part.Top;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
      monumentPart.part = MonumentPart.Part.Top;
      monumentPart.selectableStatesAndSymbols = new List<Tuple<string, string>>();
      monumentPart.stateUISymbol = "upper";
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_a", "leira"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_b", "mae"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_c", "puft"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_d", "nikola"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_e", "burt"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_f", "rowan"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_g", "nisbet"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_h", "joshua"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_i", "ren"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_j", "hatch"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_k", "drecko"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_l", "driller"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_m", "gassymoo"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_n", "glom"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_o", "lightbug"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_p", "slickster"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_q", "pacu"));
    });
  }
}
