// Decompiled with JetBrains decompiler
// Type: DebugOverlays
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class DebugOverlays : KScreen
{
  public static DebugOverlays instance { get; private set; }

  protected override void OnPrefabInit()
  {
    DebugOverlays.instance = this;
    KPopupMenu componentInChildren = this.GetComponentInChildren<KPopupMenu>();
    componentInChildren.SetOptions((IList<string>) new string[5]
    {
      "None",
      "Rooms",
      "Lighting",
      "Style",
      "Flow"
    });
    componentInChildren.OnSelect += new System.Action<string, int>(this.OnSelect);
    this.gameObject.SetActive(false);
  }

  private void OnSelect(string str, int index)
  {
    if (!(str == "None"))
    {
      if (!(str == "Flow"))
      {
        if (!(str == "Lighting"))
        {
          if (str == "Rooms")
            SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
          else
            Debug.LogError((object) ("Unknown debug view: " + str));
        }
        else
          SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
      }
      else
        SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
    }
    else
      SimDebugView.Instance.SetMode(OverlayModes.None.ID);
  }
}
