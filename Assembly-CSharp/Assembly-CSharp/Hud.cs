// Decompiled with JetBrains decompiler
// Type: Hud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Hud : KScreen
{
  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.TryConsume(Action.Help))
      return;
    GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ControlsScreen.gameObject);
  }
}
