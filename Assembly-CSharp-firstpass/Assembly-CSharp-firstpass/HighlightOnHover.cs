// Decompiled with JetBrains decompiler
// Type: HighlightOnHover
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("KMonoBehaviour/Plugins/HighlightOnHover")]
public class HighlightOnHover : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  public KImage image;

  public void OnPointerEnter(PointerEventData data) => this.image.ColorState = KImage.ColorSelector.Hover;

  public void OnPointerExit(PointerEventData data) => this.image.ColorState = KImage.ColorSelector.Inactive;
}
