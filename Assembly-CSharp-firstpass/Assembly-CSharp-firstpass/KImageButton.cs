// Decompiled with JetBrains decompiler
// Type: KImageButton
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

public class KImageButton : KButton
{
  public Text text;

  public Sprite Sprite
  {
    get => this.fgImage.sprite;
    set
    {
      this.fgImage.enabled = (Object) value != (Object) null;
      this.fgImage.sprite = value;
    }
  }

  public Sprite BackgroundSprite
  {
    get => this.bgImage.sprite;
    set
    {
      this.bgImage.enabled = (Object) value != (Object) null;
      this.bgImage.sprite = value;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.fgImage.enabled = false;
  }
}
