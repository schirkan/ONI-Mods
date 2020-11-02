// Decompiled with JetBrains decompiler
// Type: KCanvasScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/KCanvasScaler")]
public class KCanvasScaler : KMonoBehaviour
{
  [MyCmpReq]
  private CanvasScaler canvasScaler;
  public static string UIScalePrefKey = "UIScalePref";
  private float userScale = 1f;
  [Range(0.75f, 2f)]
  private KCanvasScaler.ScaleStep[] scaleSteps = new KCanvasScaler.ScaleStep[3]
  {
    new KCanvasScaler.ScaleStep(720f, 0.86f),
    new KCanvasScaler.ScaleStep(1080f, 1f),
    new KCanvasScaler.ScaleStep(2160f, 1.33f)
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (KPlayerPrefs.HasKey(KCanvasScaler.UIScalePrefKey))
      this.SetUserScale(KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey) / 100f);
    else
      this.SetUserScale(1f);
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
  }

  private void OnResize() => this.SetUserScale(this.userScale);

  public void SetUserScale(float scale)
  {
    this.userScale = scale;
    this.canvasScaler.scaleFactor = this.GetCanvasScale();
  }

  public float GetUserScale() => this.userScale;

  public float GetCanvasScale() => this.userScale * this.ScreenRelativeScale();

  private float ScreenRelativeScale()
  {
    double dpi = (double) Screen.dpi;
    Camera camera = Camera.main;
    if ((UnityEngine.Object) camera == (UnityEngine.Object) null)
      camera = UnityEngine.Object.FindObjectOfType<Camera>();
    int num = (UnityEngine.Object) camera != (UnityEngine.Object) null ? 1 : 0;
    if ((double) Screen.height <= (double) this.scaleSteps[0].maxRes_y || (double) Screen.width / (double) Screen.height < 1.6777777671814)
      return this.scaleSteps[0].scale;
    if ((double) Screen.height > (double) this.scaleSteps[this.scaleSteps.Length - 1].maxRes_y)
      return this.scaleSteps[this.scaleSteps.Length - 1].scale;
    for (int index = 0; index < this.scaleSteps.Length; ++index)
    {
      if ((double) Screen.height > (double) this.scaleSteps[index].maxRes_y && (double) Screen.height <= (double) this.scaleSteps[index + 1].maxRes_y)
      {
        float t = (float) (((double) Screen.height - (double) this.scaleSteps[index].maxRes_y) / ((double) this.scaleSteps[index + 1].maxRes_y - (double) this.scaleSteps[index].maxRes_y));
        return Mathf.Lerp(this.scaleSteps[index].scale, this.scaleSteps[index + 1].scale, t);
      }
    }
    return 1f;
  }

  [Serializable]
  public struct ScaleStep
  {
    public float scale;
    public float maxRes_y;

    public ScaleStep(float maxRes_y, float scale)
    {
      this.maxRes_y = maxRes_y;
      this.scale = scale;
    }
  }
}
