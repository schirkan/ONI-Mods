// Decompiled with JetBrains decompiler
// Type: ConversationType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ConversationType
{
  public string id;
  public string target;

  public virtual void NewTarget(MinionIdentity speaker)
  {
  }

  public virtual Conversation.Topic GetNextTopic(
    MinionIdentity speaker,
    Conversation.Topic lastTopic)
  {
    return (Conversation.Topic) null;
  }

  public virtual Sprite GetSprite(string topic) => (Sprite) null;
}
