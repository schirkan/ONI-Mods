// Decompiled with JetBrains decompiler
// Type: ControlsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

public class ControlsScreen : KScreen
{
  public Text controlLabel;

  protected override void OnPrefabInit()
  {
    BindingEntry[] bindingEntries = GameInputMapping.GetBindingEntries();
    string str = "";
    foreach (BindingEntry bindingEntry in bindingEntries)
      str = str + bindingEntry.mAction.ToString() + ": " + bindingEntry.mKeyCode.ToString() + "\n";
    this.controlLabel.text = str;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.TryConsume(Action.Help) && !e.TryConsume(Action.Escape))
      return;
    this.Deactivate();
  }
}
