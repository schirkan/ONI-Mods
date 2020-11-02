// Decompiled with JetBrains decompiler
// Type: Message
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
  public abstract string GetTitle();

  public abstract string GetSound();

  public abstract string GetMessageBody();

  public abstract string GetTooltip();

  public virtual bool ShowDialog() => true;

  public virtual void OnCleanUp()
  {
  }

  public virtual bool IsValid() => true;

  public virtual bool PlayNotificationSound() => true;

  public virtual void OnClick()
  {
  }
}
