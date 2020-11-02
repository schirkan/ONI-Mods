﻿// Decompiled with JetBrains decompiler
// Type: AnimEventHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
  private KBatchedAnimController controller;
  private KBoxCollider2D animCollider;
  private Vector3 targetPos;
  private Vector2 baseOffset;
  private HashedString context;

  private event AnimEventHandler.SetPos onWorkTargetSet;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (KBatchedAnimTracker componentsInChild in this.GetComponentsInChildren<KBatchedAnimTracker>(true))
    {
      if (componentsInChild.useTargetPoint)
        this.onWorkTargetSet += new AnimEventHandler.SetPos(componentsInChild.SetTarget);
    }
    this.controller = this.GetComponent<KBatchedAnimController>();
    this.animCollider = this.GetComponent<KBoxCollider2D>();
    this.baseOffset = this.animCollider.offset;
  }

  public HashedString GetContext() => this.context;

  public void UpdateWorkTarget(Vector3 pos)
  {
    if (this.onWorkTargetSet == null)
      return;
    this.onWorkTargetSet(pos);
  }

  public void SetContext(HashedString context) => this.context = context;

  public void SetTargetPos(Vector3 target_pos) => this.targetPos = target_pos;

  public Vector3 GetTargetPos() => this.targetPos;

  public void ClearContext() => this.context = new HashedString();

  public void LateUpdate()
  {
    Vector3 pivotSymbolPosition = this.controller.GetPivotSymbolPosition();
    this.animCollider.offset = new Vector2(this.baseOffset.x + pivotSymbolPosition.x - this.transform.GetPosition().x, this.baseOffset.y + pivotSymbolPosition.y - this.transform.GetPosition().y);
  }

  private delegate void SetPos(Vector3 pos);
}
