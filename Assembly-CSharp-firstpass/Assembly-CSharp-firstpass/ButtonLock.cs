// Decompiled with JetBrains decompiler
// Type: ButtonLock
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonLock : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
  public GameObject target;

  public void OnPointerClick(PointerEventData eventData) => this.target.SendMessage("ToggleLock", SendMessageOptions.DontRequireReceiver);

  public void OnDrag(PointerEventData eventData) => this.target.SendMessage(nameof (OnDrag), SendMessageOptions.DontRequireReceiver);

  public void OnBeginDrag(PointerEventData eventData) => this.target.SendMessage("Lock", (object) true, SendMessageOptions.DontRequireReceiver);

  public void OnEndDrag(PointerEventData eventData) => this.target.SendMessage("Lock", (object) false, SendMessageOptions.DontRequireReceiver);
}
