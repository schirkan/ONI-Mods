// Decompiled with JetBrains decompiler
// Type: Vignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class Vignette : MonoBehaviour
{
  [SerializeField]
  private Image image;
  private Color defaultColor;
  public static Vignette Instance;

  public static void DestroyInstance() => Vignette.Instance = (Vignette) null;

  private void Awake()
  {
    Vignette.Instance = this;
    this.defaultColor = this.image.color;
  }

  public void SetColor(Color color) => this.image.color = color;

  public void Reset() => this.SetColor(this.defaultColor);
}
