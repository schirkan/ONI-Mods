// Decompiled with JetBrains decompiler
// Type: SideScreenContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class SideScreenContent : KScreen
{
  [SerializeField]
  protected string titleKey;
  public GameObject ContentContainer;

  public virtual void SetTarget(GameObject target)
  {
  }

  public virtual void ClearTarget()
  {
  }

  public abstract bool IsValidForTarget(GameObject target);

  public virtual string GetTitle() => (string) Strings.Get(this.titleKey);
}
