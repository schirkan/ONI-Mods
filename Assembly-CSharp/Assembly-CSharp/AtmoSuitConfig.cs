// Decompiled with JetBrains decompiler
// Type: AtmoSuitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class AtmoSuitConfig : IEquipmentConfig
{
  public const string ID = "Atmo_Suit";
  public static ComplexRecipe recipe;
  private const PathFinder.PotentialPath.Flags suit_flags = PathFinder.PotentialPath.Flags.HasAtmoSuit;
  private AttributeModifier expertAthleticsModifier;

  public EquipmentDef CreateEquipmentDef()
  {
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    AttributeModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_DIGGING, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    this.expertAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) -TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS, Db.Get().Skills.Suits1.Name);
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Atmo_Suit", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Dirt, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_MASS, "suit_oxygen_kanim", "", "body_oxygen_kanim", 6, AttributeModifiers, IsBody: true, additional_tags: new Tag[1]
    {
      GameTags.Suit
    });
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC;
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("SoakingWet"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("PoppedEarDrums"));
    equipmentDef.OnEquipCallBack = (System.Action<Equippable>) (eq =>
    {
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      Navigator component1 = targetGameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit);
      MinionResume component2 = targetGameObject.GetComponent<MinionResume>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || !component2.HasPerk((HashedString) Db.Get().SkillPerks.ExosuitExpertise.Id))
        return;
      targetGameObject.GetAttributes().Get(Db.Get().Attributes.Athletics).Add(this.expertAthleticsModifier);
    });
    equipmentDef.OnUnequipCallBack = (System.Action<Equippable>) (eq =>
    {
      if (eq.assignee == null)
        return;
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if ((bool) (UnityEngine.Object) targetGameObject)
      {
        targetGameObject.GetAttributes()?.Get(Db.Get().Attributes.Athletics).Remove(this.expertAthleticsModifier);
        Navigator component1 = targetGameObject.GetComponent<Navigator>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          component1.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit);
        Effects component2 = targetGameObject.GetComponent<Effects>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.HasEffect("SoiledSuit"))
          component2.Remove("SoiledSuit");
      }
      eq.GetComponent<Storage>().DropAll(eq.transform.GetPosition(), true, true, do_disease_transfer: false);
    });
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Atmo_Suit");
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Helmet");
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    SuitTank suitTank = go.AddComponent<SuitTank>();
    suitTank.element = "Oxygen";
    suitTank.capacity = 75f;
    go.AddComponent<HelmetController>();
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Clothes);
    component.AddTag(GameTags.PedestalDisplayable);
    component.AddTag(GameTags.AirtightSuit);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    storage.showInUI = true;
    go.AddOrGet<AtmoSuit>();
    go.AddComponent<SuitDiseaseHandler>();
  }
}
