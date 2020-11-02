// Decompiled with JetBrains decompiler
// Type: LogicRibbonDisplayUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicRibbonDisplayUI : KMonoBehaviour
{
  [SerializeField]
  private Image wire1;
  [SerializeField]
  private Image wire2;
  [SerializeField]
  private Image wire3;
  [SerializeField]
  private Image wire4;
  [SerializeField]
  private LogicModeUI uiAsset;
  private Color32 colourOn;
  private Color32 colourOff;
  private Color32 colourDisconnected = (Color32) new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue);
  private int bitDepth = 4;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
    this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
    this.colourOn.a = this.colourOff.a = byte.MaxValue;
    this.wire1.raycastTarget = false;
    this.wire2.raycastTarget = false;
    this.wire3.raycastTarget = false;
    this.wire4.raycastTarget = false;
  }

  public void SetContent(LogicCircuitNetwork network)
  {
    Color32 colourDisconnected = this.colourDisconnected;
    List<Color32> color32List = new List<Color32>();
    for (int bit = 0; bit < this.bitDepth; ++bit)
      color32List.Add(network == null ? colourDisconnected : (network.IsBitActive(bit) ? this.colourOn : this.colourOff));
    if (this.wire1.color != (Color) color32List[0])
      this.wire1.color = (Color) color32List[0];
    if (this.wire2.color != (Color) color32List[1])
      this.wire2.color = (Color) color32List[1];
    if (this.wire3.color != (Color) color32List[2])
      this.wire3.color = (Color) color32List[2];
    if (!(this.wire4.color != (Color) color32List[3]))
      return;
    this.wire4.color = (Color) color32List[3];
  }
}
