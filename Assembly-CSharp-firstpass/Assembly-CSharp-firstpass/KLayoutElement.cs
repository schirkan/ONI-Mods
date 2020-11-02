// Decompiled with JetBrains decompiler
// Type: KLayoutElement
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine.UI;

public class KLayoutElement : LayoutElement
{
  public bool makeDirtyOnDisable = true;
  private bool hasEnabledOnce;

  protected override void OnEnable()
  {
    bool flag = this.makeDirtyOnDisable;
    if (!this.hasEnabledOnce)
    {
      this.hasEnabledOnce = true;
      flag = true;
    }
    if (!flag)
      return;
    base.OnEnable();
  }

  protected override void OnDisable()
  {
    if (!this.makeDirtyOnDisable)
      return;
    base.OnDisable();
  }
}
