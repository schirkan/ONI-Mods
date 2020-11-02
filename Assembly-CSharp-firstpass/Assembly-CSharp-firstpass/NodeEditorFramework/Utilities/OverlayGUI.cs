// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Utilities.OverlayGUI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace NodeEditorFramework.Utilities
{
  public static class OverlayGUI
  {
    public static PopupMenu currentPopup;

    public static bool HasPopupControl() => OverlayGUI.currentPopup != null;

    public static void StartOverlayGUI()
    {
      if (OverlayGUI.currentPopup == null || Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
        return;
      OverlayGUI.currentPopup.Draw();
    }

    public static void EndOverlayGUI()
    {
      if (OverlayGUI.currentPopup == null || Event.current.type != EventType.Layout && Event.current.type != EventType.Repaint)
        return;
      OverlayGUI.currentPopup.Draw();
    }
  }
}
