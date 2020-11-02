// Decompiled with JetBrains decompiler
// Type: MinionVitalsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MinionVitalsScreen : TargetScreen
{
  public MinionVitalsPanel panel;

  public override bool IsValidForTarget(GameObject target) => (bool) (Object) target.GetComponent<MinionIdentity>();

  public override void ScreenUpdate(bool topLevel) => base.ScreenUpdate(topLevel);

  public override void OnSelectTarget(GameObject target)
  {
    this.panel.selectedEntity = target;
    this.panel.Refresh();
  }

  public override void OnDeselectTarget(GameObject target)
  {
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    if ((Object) this.panel == (Object) null)
      this.panel = this.GetComponent<MinionVitalsPanel>();
    this.panel.Init();
  }
}
