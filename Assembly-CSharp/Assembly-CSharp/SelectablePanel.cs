// Decompiled with JetBrains decompiler
// Type: SelectablePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class SelectablePanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
  public void OnDeselect(BaseEventData evt) => this.gameObject.SetActive(false);
}
