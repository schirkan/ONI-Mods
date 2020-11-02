// Decompiled with JetBrains decompiler
// Type: GameOverScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class GameOverScreen : KModalScreen
{
  public KButton DismissButton;
  public KButton QuitButton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Init();
  }

  private void Init()
  {
    if ((bool) (UnityEngine.Object) this.QuitButton)
      this.QuitButton.onClick += (System.Action) (() => this.Quit());
    if (!(bool) (UnityEngine.Object) this.DismissButton)
      return;
    this.DismissButton.onClick += (System.Action) (() => this.Dismiss());
  }

  private void Quit() => PauseScreen.TriggerQuitGame();

  private void Dismiss() => this.Show(false);
}
