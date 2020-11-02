// Decompiled with JetBrains decompiler
// Type: TransitionDriver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TransitionDriver
{
  private Navigator.ActiveTransition transition;
  private Navigator navigator;
  private Vector3 targetPos;
  private bool isComplete;
  private Brain brain;
  public List<TransitionDriver.OverrideLayer> overrideLayers = new List<TransitionDriver.OverrideLayer>();
  private LoggerFS log;

  public Navigator.ActiveTransition GetTransition => this.transition;

  public TransitionDriver(Navigator navigator) => this.log = new LoggerFS(nameof (TransitionDriver));

  public void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
      overrideLayer.BeginTransition(navigator, transition);
    this.navigator = navigator;
    this.transition = transition;
    this.isComplete = false;
    Grid.SceneLayer layer = navigator.sceneLayer;
    if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
      layer = Grid.SceneLayer.BuildingUse;
    else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
    {
      KBatchedAnimController component = navigator.GetComponent<KBatchedAnimController>();
      layer = Grid.SceneLayer.FXFront;
      int num = (int) layer;
      component.SetSceneLayer((Grid.SceneLayer) num);
    }
    else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
      navigator.GetComponent<KBatchedAnimController>().SetSceneLayer(layer);
    this.targetPos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) navigator), transition.x, transition.y), layer);
    if (transition.isLooping)
    {
      KAnimControllerBase component = navigator.GetComponent<KAnimControllerBase>();
      component.PlaySpeedMultiplier = transition.animSpeed;
      bool flag1 = transition.preAnim != (HashedString) "";
      bool flag2 = component.CurrentAnim != null && (HashedString) component.CurrentAnim.name == transition.anim;
      if ((!flag1 || component.CurrentAnim == null ? 0 : ((HashedString) component.CurrentAnim.name == transition.preAnim ? 1 : 0)) != 0)
      {
        component.ClearQueue();
        component.Queue(transition.anim, KAnim.PlayMode.Loop);
      }
      else if (flag2)
      {
        if (component.PlayMode != KAnim.PlayMode.Loop)
        {
          component.ClearQueue();
          component.Queue(transition.anim, KAnim.PlayMode.Loop);
        }
      }
      else if (flag1)
      {
        component.Play(transition.preAnim);
        component.Queue(transition.anim, KAnim.PlayMode.Loop);
      }
      else
        component.Play(transition.anim, KAnim.PlayMode.Loop);
    }
    else if (transition.anim != (HashedString) (string) null)
    {
      KAnimControllerBase component = navigator.GetComponent<KAnimControllerBase>();
      component.PlaySpeedMultiplier = transition.animSpeed;
      component.Play(transition.anim);
      navigator.Subscribe(-1061186183, new System.Action<object>(this.OnAnimComplete));
    }
    if (transition.navGridTransition.y != (sbyte) 0)
    {
      if (transition.navGridTransition.start == NavType.RightWall)
        navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.y < (sbyte) 0);
      else if (transition.navGridTransition.start == NavType.LeftWall)
        navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.y > (sbyte) 0);
    }
    if (transition.navGridTransition.x != (sbyte) 0)
    {
      if (transition.navGridTransition.start == NavType.Ceiling)
        navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.x > (sbyte) 0);
      else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
        navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.x < (sbyte) 0);
    }
    this.brain = navigator.GetComponent<Brain>();
  }

  public void UpdateTransition(float dt)
  {
    if ((UnityEngine.Object) this.navigator == (UnityEngine.Object) null)
      return;
    foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
      overrideLayer.UpdateTransition(this.navigator, this.transition);
    if (!this.isComplete && this.transition.isCompleteCB != null)
      this.isComplete = this.transition.isCompleteCB();
    if ((UnityEngine.Object) this.brain != (UnityEngine.Object) null)
    {
      int num = this.isComplete ? 1 : 0;
    }
    if (this.transition.isLooping)
    {
      float speed = this.transition.speed;
      Vector3 position = this.navigator.transform.GetPosition();
      if (this.transition.x > 0)
      {
        position.x += dt * speed;
        if ((double) position.x > (double) this.targetPos.x)
          this.isComplete = true;
      }
      else if (this.transition.x < 0)
      {
        position.x -= dt * speed;
        if ((double) position.x < (double) this.targetPos.x)
          this.isComplete = true;
      }
      else
        position.x = this.targetPos.x;
      if (this.transition.y > 0)
      {
        position.y += dt * speed;
        if ((double) position.y > (double) this.targetPos.y)
          this.isComplete = true;
      }
      else if (this.transition.y < 0)
      {
        position.y -= dt * speed;
        if ((double) position.y < (double) this.targetPos.y)
          this.isComplete = true;
      }
      else
        position.y = this.targetPos.y;
      this.navigator.transform.SetPosition(position);
    }
    if (!this.isComplete)
      return;
    this.isComplete = false;
    Navigator navigator = this.navigator;
    navigator.SetCurrentNavType(this.transition.end);
    navigator.transform.SetPosition(this.targetPos);
    this.EndTransition();
    navigator.AdvancePath();
  }

  private void OnAnimComplete(object data)
  {
    if ((UnityEngine.Object) this.navigator != (UnityEngine.Object) null)
      this.navigator.Unsubscribe(-1061186183, new System.Action<object>(this.OnAnimComplete));
    this.isComplete = true;
  }

  public void EndTransition()
  {
    if (!((UnityEngine.Object) this.navigator != (UnityEngine.Object) null))
      return;
    Navigator navigator = this.navigator;
    foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
      overrideLayer.EndTransition(this.navigator, this.transition);
    this.navigator = (Navigator) null;
    navigator.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 1f;
    navigator.Unsubscribe(-1061186183, new System.Action<object>(this.OnAnimComplete));
    Brain component = navigator.GetComponent<Brain>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Resume("move_handler");
  }

  public class OverrideLayer
  {
    public OverrideLayer(Navigator navigator)
    {
    }

    public virtual void Destroy()
    {
    }

    public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
    }

    public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
    }

    public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
    }
  }
}
