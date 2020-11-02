// Decompiled with JetBrains decompiler
// Type: Tween
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tween : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  private static float Scale = 1.025f;
  private static float ScaleSpeed = 0.5f;
  private Selectable Selectable;
  private float Direction = -1f;

  private void Awake() => this.Selectable = this.GetComponent<Selectable>();

  public void OnPointerEnter(PointerEventData data) => this.Direction = 1f;

  public void OnPointerExit(PointerEventData data) => this.Direction = -1f;

  private void Update()
  {
    if (!this.Selectable.interactable)
      return;
    float x = this.transform.localScale.x;
    float num = Mathf.Max(Mathf.Min(x + this.Direction * Time.unscaledDeltaTime * Tween.ScaleSpeed, Tween.Scale), 1f);
    if ((double) num == (double) x)
      return;
    this.transform.localScale = new Vector3(num, num, 1f);
  }
}
