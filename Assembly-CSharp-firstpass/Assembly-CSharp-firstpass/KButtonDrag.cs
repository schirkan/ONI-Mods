// Decompiled with JetBrains decompiler
// Type: KButtonDrag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine.EventSystems;

public class KButtonDrag : KButton, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
  public event System.Action onBeginDrag;

  public event System.Action onDrag;

  public event System.Action onEndDrag;

  public void ClearOnDragEvents()
  {
    this.onBeginDrag = (System.Action) null;
    this.onDrag = (System.Action) null;
    this.onEndDrag = (System.Action) null;
  }

  public void OnBeginDrag(PointerEventData data) => this.onBeginDrag.Signal();

  public void OnDrag(PointerEventData data) => this.onDrag.Signal();

  public void OnEndDrag(PointerEventData data) => this.onEndDrag.Signal();
}
