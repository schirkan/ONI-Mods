// Decompiled with JetBrains decompiler
// Type: StarmapPlanetVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanetVisualizer")]
public class StarmapPlanetVisualizer : KMonoBehaviour
{
  public Image image;
  public LocText label;
  public MultiToggle button;
  public RectTransform selection;
  public GameObject analysisSelection;
  public Image unknownBG;
  public GameObject rocketIconContainer;
}
