// Decompiled with JetBrains decompiler
// Type: IncubatorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class IncubatorSideScreen : ReceptacleSideScreen
{
  public DescriptorPanel RequirementsDescriptorPanel;
  public DescriptorPanel HarvestDescriptorPanel;
  public DescriptorPanel EffectsDescriptorPanel;
  public MultiToggle continuousToggle;

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<EggIncubator>() != (UnityEngine.Object) null;

  protected override void SetResultDescriptions(GameObject go)
  {
    string text = "";
    InfoDescription component = go.GetComponent<InfoDescription>();
    if ((bool) (UnityEngine.Object) component)
      text += component.description;
    this.descriptionLabel.SetText(text);
  }

  protected override bool RequiresAvailableAmountToDeposit() => false;

  protected override Sprite GetEntityIcon(Tag prefabTag) => Def.GetUISprite((object) Assets.GetPrefab(prefabTag)).first;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    EggIncubator incubator = target.GetComponent<EggIncubator>();
    this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
    this.continuousToggle.onClick = (System.Action) (() =>
    {
      incubator.autoReplaceEntity = !incubator.autoReplaceEntity;
      this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
    });
  }
}
