// Decompiled with JetBrains decompiler
// Type: CodexImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexImage : CodexWidget<CodexImage>
{
  public Sprite sprite { get; set; }

  public Color color { get; set; }

  public string spriteName
  {
    set => this.sprite = Assets.GetSprite((HashedString) value);
    get => "--> " + ((Object) this.sprite == (Object) null ? "NULL" : this.sprite.ToString());
  }

  public string batchedAnimPrefabSourceID
  {
    set
    {
      GameObject prefab = Assets.GetPrefab((Tag) value);
      KBatchedAnimController kbatchedAnimController = (Object) prefab != (Object) null ? prefab.GetComponent<KBatchedAnimController>() : (KBatchedAnimController) null;
      KAnimFile animFile = (Object) kbatchedAnimController != (Object) null ? kbatchedAnimController.AnimFiles[0] : (KAnimFile) null;
      this.sprite = (Object) animFile != (Object) null ? Def.GetUISpriteFromMultiObjectAnim(animFile) : (Sprite) null;
    }
    get => "--> " + ((Object) this.sprite == (Object) null ? "NULL" : this.sprite.ToString());
  }

  public CodexImage() => this.color = Color.white;

  public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite, Color color)
    : base(preferredWidth, preferredHeight)
  {
    this.sprite = sprite;
    this.color = color;
  }

  public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite)
    : this(preferredWidth, preferredHeight, sprite, Color.white)
  {
  }

  public CodexImage(int preferredWidth, int preferredHeight, Tuple<Sprite, Color> coloredSprite)
    : this(preferredWidth, preferredHeight, coloredSprite.first, coloredSprite.second)
  {
  }

  public CodexImage(Tuple<Sprite, Color> coloredSprite)
    : this(-1, -1, coloredSprite)
  {
  }

  public void ConfigureImage(Image image)
  {
    image.sprite = this.sprite;
    image.color = this.color;
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    this.ConfigureImage(contentGameObject.GetComponent<Image>());
    this.ConfigurePreferredLayout(contentGameObject);
  }
}
