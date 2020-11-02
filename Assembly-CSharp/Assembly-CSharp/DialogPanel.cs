// Decompiled with JetBrains decompiler
// Type: DialogPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
  public bool destroyOnDeselect = true;

  public void OnDeselect(BaseEventData eventData)
  {
    if (this.destroyOnDeselect)
    {
      foreach (Component component in this.transform)
        Util.KDestroyGameObject(component.gameObject);
    }
    this.gameObject.SetActive(false);
  }
}
