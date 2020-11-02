// Decompiled with JetBrains decompiler
// Type: ImageToggleState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/Plugins/ImageToggleState")]
public class ImageToggleState : KMonoBehaviour
{
  public Image TargetImage;
  public Sprite ActiveSprite;
  public Sprite InactiveSprite;
  public Sprite DisabledSprite;
  public Sprite DisabledActiveSprite;
  public bool useSprites;
  public Color ActiveColour = Color.white;
  public Color InactiveColour = Color.white;
  public Color DisabledColour = Color.white;
  public Color DisabledActiveColour = Color.white;
  public Color HoverColour = Color.white;
  public Color DisabledHoverColor = Color.white;
  public ColorStyleSetting colorStyleSetting;
  private bool isActive;
  private ImageToggleState.State currentState = ImageToggleState.State.Inactive;
  public bool useStartingState;
  public ImageToggleState.State startingState = ImageToggleState.State.Inactive;

  public bool IsDisabled => this.currentState == ImageToggleState.State.Disabled || this.currentState == ImageToggleState.State.DisabledActive;

  public new void Awake()
  {
    base.Awake();
    this.RefreshColorStyle();
    if (!this.useStartingState)
      return;
    this.SetState(this.startingState);
  }

  [ContextMenu("Refresh Colour Style")]
  public void RefreshColorStyle()
  {
    if (!((Object) this.colorStyleSetting != (Object) null))
      return;
    this.ActiveColour = this.colorStyleSetting.activeColor;
    this.InactiveColour = this.colorStyleSetting.inactiveColor;
    this.DisabledColour = this.colorStyleSetting.disabledColor;
    this.DisabledActiveColour = this.colorStyleSetting.disabledActiveColor;
    this.HoverColour = this.colorStyleSetting.hoverColor;
    this.DisabledHoverColor = this.colorStyleSetting.disabledhoverColor;
  }

  public void SetSprites(Sprite disabled, Sprite inactive, Sprite active, Sprite disabledActive)
  {
    if ((Object) disabled != (Object) null)
      this.DisabledSprite = disabled;
    if ((Object) inactive != (Object) null)
      this.InactiveSprite = inactive;
    if ((Object) active != (Object) null)
      this.ActiveSprite = active;
    if ((Object) disabledActive != (Object) null)
      this.DisabledActiveSprite = disabledActive;
    this.useSprites = true;
  }

  public bool GetIsActive() => this.isActive;

  private void SetTargetImageColor(Color color) => this.TargetImage.color = color;

  public void SetState(ImageToggleState.State newState)
  {
    if (this.currentState == newState)
      return;
    switch (newState)
    {
      case ImageToggleState.State.Disabled:
        this.SetDisabled();
        break;
      case ImageToggleState.State.Inactive:
        this.SetInactive();
        break;
      case ImageToggleState.State.Active:
        this.SetActive();
        break;
      case ImageToggleState.State.DisabledActive:
        this.SetDisabledActive();
        break;
    }
  }

  public void SetActiveState(bool active)
  {
    if (active)
      this.SetActive();
    else
      this.SetInactive();
  }

  public void SetActive()
  {
    if (this.currentState == ImageToggleState.State.Active)
      return;
    this.isActive = true;
    this.currentState = ImageToggleState.State.Active;
    if ((Object) this.TargetImage == (Object) null)
      return;
    this.SetTargetImageColor(this.ActiveColour);
    if (!this.useSprites)
      return;
    if ((Object) this.ActiveSprite != (Object) null && (Object) this.TargetImage.sprite != (Object) this.ActiveSprite)
    {
      this.TargetImage.sprite = this.ActiveSprite;
    }
    else
    {
      if (!((Object) this.ActiveSprite == (Object) null))
        return;
      this.TargetImage.sprite = (Sprite) null;
    }
  }

  public void SetColorStyle(ColorStyleSetting style)
  {
    this.colorStyleSetting = style;
    this.RefreshColorStyle();
    this.ResetColor();
  }

  public void ResetColor()
  {
    switch (this.currentState)
    {
      case ImageToggleState.State.Disabled:
        this.SetTargetImageColor(this.DisabledColour);
        break;
      case ImageToggleState.State.Inactive:
        this.SetTargetImageColor(this.InactiveColour);
        break;
      case ImageToggleState.State.Active:
        this.SetTargetImageColor(this.ActiveColour);
        break;
      case ImageToggleState.State.DisabledActive:
        this.SetTargetImageColor(this.DisabledActiveColour);
        break;
    }
  }

  public void OnHoverIn() => this.SetTargetImageColor(this.currentState == ImageToggleState.State.Disabled || this.currentState == ImageToggleState.State.DisabledActive ? this.DisabledHoverColor : this.HoverColour);

  public void OnHoverOut() => this.ResetColor();

  public void SetInactive()
  {
    if (this.currentState == ImageToggleState.State.Inactive)
      return;
    this.isActive = false;
    this.currentState = ImageToggleState.State.Inactive;
    this.SetTargetImageColor(this.InactiveColour);
    if ((Object) this.TargetImage == (Object) null || !this.useSprites)
      return;
    if ((Object) this.InactiveSprite != (Object) null && (Object) this.TargetImage.sprite != (Object) this.InactiveSprite)
    {
      this.TargetImage.sprite = this.InactiveSprite;
    }
    else
    {
      if (!((Object) this.InactiveSprite == (Object) null))
        return;
      this.TargetImage.sprite = (Sprite) null;
    }
  }

  public void SetDisabled()
  {
    if (this.currentState == ImageToggleState.State.Disabled)
    {
      this.SetTargetImageColor(this.DisabledColour);
    }
    else
    {
      this.isActive = false;
      this.currentState = ImageToggleState.State.Disabled;
      this.SetTargetImageColor(this.DisabledColour);
      if ((Object) this.TargetImage == (Object) null || !this.useSprites)
        return;
      if ((Object) this.DisabledSprite != (Object) null && (Object) this.TargetImage.sprite != (Object) this.DisabledSprite)
      {
        this.TargetImage.sprite = this.DisabledSprite;
      }
      else
      {
        if (!((Object) this.DisabledSprite == (Object) null))
          return;
        this.TargetImage.sprite = (Sprite) null;
      }
    }
  }

  public void SetDisabledActive()
  {
    this.isActive = false;
    this.currentState = ImageToggleState.State.DisabledActive;
    if ((Object) this.TargetImage == (Object) null)
      return;
    this.SetTargetImageColor(this.DisabledActiveColour);
    if (!this.useSprites)
      return;
    if ((Object) this.DisabledActiveSprite != (Object) null && (Object) this.TargetImage.sprite != (Object) this.DisabledActiveSprite)
    {
      this.TargetImage.sprite = this.DisabledActiveSprite;
    }
    else
    {
      if (!((Object) this.DisabledActiveSprite == (Object) null))
        return;
      this.TargetImage.sprite = (Sprite) null;
    }
  }

  public enum State
  {
    Disabled,
    Inactive,
    Active,
    DisabledActive,
  }
}
