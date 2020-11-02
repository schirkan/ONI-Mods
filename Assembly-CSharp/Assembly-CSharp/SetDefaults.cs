// Decompiled with JetBrains decompiler
// Type: SetDefaults
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class SetDefaults
{
  public static void Initialize()
  {
    KSlider.DefaultSounds[0] = GlobalAssets.GetSound("Slider_Start");
    KSlider.DefaultSounds[1] = GlobalAssets.GetSound("Slider_Move");
    KSlider.DefaultSounds[2] = GlobalAssets.GetSound("Slider_End");
    KSlider.DefaultSounds[3] = GlobalAssets.GetSound("Slider_Boundary_Low");
    KSlider.DefaultSounds[4] = GlobalAssets.GetSound("Slider_Boundary_High");
    KScrollRect.DefaultSounds[KScrollRect.SoundType.OnMouseScroll] = GlobalAssets.GetSound("Mousewheel_Move");
    WidgetSoundPlayer.getSoundPath = new Func<string, string>(SetDefaults.GetSoundPath);
  }

  private static string GetSoundPath(string sound_name) => GlobalAssets.GetSound(sound_name);
}
