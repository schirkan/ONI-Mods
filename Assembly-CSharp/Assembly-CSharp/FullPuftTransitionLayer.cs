// Decompiled with JetBrains decompiler
// Type: FullPuftTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
  public FullPuftTransitionLayer(Navigator navigator)
    : base(navigator)
  {
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    CreatureCalorieMonitor.Instance smi = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
    if (smi == null || !smi.stomach.IsReadyToPoop())
      return;
    KBatchedAnimController component = navigator.GetComponent<KBatchedAnimController>();
    string str = HashCache.Get().Get(transition.anim.HashValue) + "_full";
    HashedString anim_name = (HashedString) str;
    if (!component.HasAnimation(anim_name))
      return;
    transition.anim = (HashedString) str;
  }
}
