// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Utilities.GenericMenu
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace NodeEditorFramework.Utilities
{
  public class GenericMenu
  {
    private static PopupMenu popup;

    public Vector2 Position => GenericMenu.popup.Position;

    public GenericMenu() => GenericMenu.popup = new PopupMenu();

    public void ShowAsContext() => GenericMenu.popup.Show(GUIScaleUtility.GUIToScreenSpace(Event.current.mousePosition));

    public void Show(Vector2 pos, float MinWidth = 40f) => GenericMenu.popup.Show(GUIScaleUtility.GUIToScreenSpace(pos), MinWidth);

    public void AddItem(
      GUIContent content,
      bool on,
      PopupMenu.MenuFunctionData func,
      object userData)
    {
      GenericMenu.popup.AddItem(content, on, func, userData);
    }

    public void AddItem(GUIContent content, bool on, PopupMenu.MenuFunction func) => GenericMenu.popup.AddItem(content, on, func);

    public void AddSeparator(string path) => GenericMenu.popup.AddSeparator(path);
  }
}
