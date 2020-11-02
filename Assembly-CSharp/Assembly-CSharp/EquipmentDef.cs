// Decompiled with JetBrains decompiler
// Type: EquipmentDef
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

public class EquipmentDef : Def
{
  public string Id;
  public string Slot;
  public string FabricatorId;
  public float FabricationTime;
  public string RecipeTechUnlock;
  public SimHashes OutputElement;
  public Dictionary<string, float> InputElementMassMap;
  public float Mass;
  public KAnimFile Anim;
  public string SnapOn;
  public string SnapOn1;
  public KAnimFile BuildOverride;
  public int BuildOverridePriority;
  public bool IsBody;
  public List<AttributeModifier> AttributeModifiers;
  public string RecipeDescription;
  public List<Effect> EffectImmunites = new List<Effect>();
  public System.Action<Equippable> OnEquipCallBack;
  public System.Action<Equippable> OnUnequipCallBack;
  public EntityTemplates.CollisionShape CollisionShape;
  public float width;
  public float height = 0.325f;
  public Tag[] AdditionalTags;
  public List<Descriptor> additionalDescriptors = new List<Descriptor>();

  public override string Name => (string) Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".NAME");

  public string GenericName => (string) Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".GENERICNAME");
}
