// Decompiled with JetBrains decompiler
// Type: RestartWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class RestartWarning : MonoBehaviour
{
  public static bool ShouldWarn;
  public LocText text;
  public Image image;

  private void Update()
  {
    if (!RestartWarning.ShouldWarn)
      return;
    this.text.enabled = true;
    this.image.enabled = true;
  }
}
