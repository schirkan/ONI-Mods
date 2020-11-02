// Decompiled with JetBrains decompiler
// Type: CancellableDig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class CancellableDig : Cancellable
{
  protected override void OnCancel(object data)
  {
    EasingAnimations componentInChildren = this.GetComponentInChildren<EasingAnimations>();
    componentInChildren.OnAnimationDone += new System.Action<string>(this.OnAnimationDone);
    componentInChildren.PlayAnimation("ScaleDown", 0.1f);
  }

  private void OnAnimationDone(string animationName)
  {
    if (animationName != "ScaleDown")
      return;
    this.GetComponentInChildren<EasingAnimations>().OnAnimationDone -= new System.Action<string>(this.OnAnimationDone);
    this.DeleteObject();
  }
}
