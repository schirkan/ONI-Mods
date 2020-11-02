// Decompiled with JetBrains decompiler
// Type: KInputBinding
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class KInputBinding
{
  public KKeyCode mKeyCode;
  public Action mAction;
  public Modifier mModifier;

  public KInputBinding(KKeyCode key_code, Modifier modifier, Action action)
  {
    this.mKeyCode = key_code;
    this.mAction = action;
    this.mModifier = modifier;
  }
}
