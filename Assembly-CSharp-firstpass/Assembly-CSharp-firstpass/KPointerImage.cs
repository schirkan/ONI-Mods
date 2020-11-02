// Decompiled with JetBrains decompiler
// Type: KPointerImage
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine.EventSystems;

public class KPointerImage : KImage, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
  public event System.Action onPointerEnter;

  public event System.Action onPointerExit;

  public event System.Action onPointerDown;

  public event System.Action onPointerUp;

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (this.onPointerEnter == null)
      return;
    this.onPointerEnter();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (this.onPointerExit == null)
      return;
    this.onPointerExit();
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (this.onPointerDown == null)
      return;
    this.onPointerDown();
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (this.onPointerUp == null)
      return;
    this.onPointerUp();
  }

  public void ClearPointerEvents()
  {
    this.onPointerEnter = (System.Action) null;
    this.onPointerExit = (System.Action) null;
    this.onPointerDown = (System.Action) null;
    this.onPointerUp = (System.Action) null;
  }
}
