// Decompiled with JetBrains decompiler
// Type: ScheduleScreenColumnEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScheduleScreenColumnEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
{
  public Image image;
  public System.Action onLeftClick;

  public void OnPointerEnter(PointerEventData event_data) => this.RunCallbacks();

  private void RunCallbacks()
  {
    if (!Input.GetMouseButton(0) || this.onLeftClick == null)
      return;
    this.onLeftClick();
  }

  public void OnPointerDown(PointerEventData event_data) => this.RunCallbacks();
}
