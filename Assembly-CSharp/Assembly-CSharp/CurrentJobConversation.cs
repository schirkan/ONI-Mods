﻿// Decompiled with JetBrains decompiler
// Type: CurrentJobConversation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CurrentJobConversation : ConversationType
{
  public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>()
  {
    {
      Conversation.ModeType.Query,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Statement
      }
    },
    {
      Conversation.ModeType.Satisfaction,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Agreement
      }
    },
    {
      Conversation.ModeType.Nominal,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Musing
      }
    },
    {
      Conversation.ModeType.Dissatisfaction,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Disagreement
      }
    },
    {
      Conversation.ModeType.Stressing,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Disagreement
      }
    },
    {
      Conversation.ModeType.Agreement,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Query,
        Conversation.ModeType.End
      }
    },
    {
      Conversation.ModeType.Disagreement,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Query,
        Conversation.ModeType.End
      }
    },
    {
      Conversation.ModeType.Musing,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Query,
        Conversation.ModeType.End
      }
    }
  };

  public CurrentJobConversation() => this.id = nameof (CurrentJobConversation);

  public override void NewTarget(MinionIdentity speaker) => this.target = "hows_role";

  public override Conversation.Topic GetNextTopic(
    MinionIdentity speaker,
    Conversation.Topic lastTopic)
  {
    if (lastTopic == null)
      return new Conversation.Topic(this.target, Conversation.ModeType.Query);
    List<Conversation.ModeType> transition = CurrentJobConversation.transitions[lastTopic.mode];
    Conversation.ModeType mode = transition[Random.Range(0, transition.Count)];
    if (mode != Conversation.ModeType.Statement)
      return new Conversation.Topic(this.target, mode);
    this.target = this.GetRoleForSpeaker(speaker);
    return new Conversation.Topic(this.target, this.GetModeForRole(speaker, this.target));
  }

  public override Sprite GetSprite(string topic)
  {
    if (topic == "hows_role")
      return Assets.GetSprite((HashedString) "crew_state_role");
    return Db.Get().Skills.TryGet(topic) != null ? Assets.GetSprite((HashedString) Db.Get().Skills.Get(topic).hat) : (Sprite) null;
  }

  private Conversation.ModeType GetModeForRole(MinionIdentity speaker, string roleId) => Conversation.ModeType.Nominal;

  private string GetRoleForSpeaker(MinionIdentity speaker) => speaker.GetComponent<MinionResume>().CurrentRole;
}