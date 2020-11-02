// Decompiled with JetBrains decompiler
// Type: MessageDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class MessageDialog : KMonoBehaviour
{
  public virtual bool CanDontShowAgain => false;

  public abstract bool CanDisplay(Message message);

  public abstract void SetMessage(Message message);

  public abstract void OnClickAction();

  public virtual void OnDontShowAgain()
  {
  }
}
