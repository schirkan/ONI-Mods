﻿// Decompiled with JetBrains decompiler
// Type: ConversationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

public class ConversationMonitor : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>
{
  private const int MAX_RECENT_TOPICS = 5;
  private const int MAX_FAVOURITE_TOPICS = 5;
  private const float FAVOURITE_CHANCE = 0.03333334f;
  private const float LEARN_CHANCE = 0.3333333f;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventHandler(GameHashes.TopicDiscussed, (GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameEvent.Callback) ((smi, obj) => smi.OnTopicDiscussed(obj))).EventHandler(GameHashes.TopicDiscovered, (GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameEvent.Callback) ((smi, obj) => smi.OnTopicDiscovered(obj)));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public new class Instance : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameInstance
  {
    [Serialize]
    private Queue<string> recentTopics;
    [Serialize]
    private List<string> favouriteTopics;
    private List<string> personalTopics;
    private static readonly List<string> randomTopics = new List<string>()
    {
      "Headquarters"
    };

    public Instance(IStateMachineTarget master, ConversationMonitor.Def def)
      : base(master, def)
    {
      this.recentTopics = new Queue<string>();
      this.favouriteTopics = new List<string>()
      {
        ConversationMonitor.Instance.randomTopics[Random.Range(0, ConversationMonitor.Instance.randomTopics.Count)]
      };
      this.personalTopics = new List<string>();
    }

    public string GetATopic()
    {
      int num = Random.Range(0, this.recentTopics.Count + this.favouriteTopics.Count * 2 + this.personalTopics.Count);
      if (num < this.recentTopics.Count)
        return this.recentTopics.Dequeue();
      int index1 = num - this.recentTopics.Count;
      if (index1 < this.favouriteTopics.Count)
        return this.favouriteTopics[index1];
      int index2 = index1 - this.favouriteTopics.Count;
      if (index2 < this.favouriteTopics.Count)
        return this.favouriteTopics[index2];
      int index3 = index2 - this.favouriteTopics.Count;
      return index3 < this.personalTopics.Count ? this.personalTopics[index3] : "";
    }

    public void OnTopicDiscovered(object data)
    {
      string str = (string) data;
      if (this.recentTopics.Contains(str))
        return;
      this.recentTopics.Enqueue(str);
      if (this.recentTopics.Count <= 5)
        return;
      this.TryMakeFavouriteTopic(this.recentTopics.Dequeue());
    }

    public void OnTopicDiscussed(object data)
    {
      string str = (string) data;
      if ((double) Random.value >= 0.333333343267441)
        return;
      this.OnTopicDiscovered((object) str);
    }

    private void TryMakeFavouriteTopic(string topic)
    {
      if ((double) Random.value >= 0.0333333350718021)
        return;
      if (this.favouriteTopics.Count < 5)
        this.favouriteTopics.Add(topic);
      else
        this.favouriteTopics[Random.Range(0, this.favouriteTopics.Count)] = topic;
    }
  }
}