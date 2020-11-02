﻿// Decompiled with JetBrains decompiler
// Type: ConsumableConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
  [Serialize]
  public Tag[] forbiddenTags;
  public System.Action consumableRulesChanged;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if ((UnityEngine.Object) ConsumerManager.instance != (UnityEngine.Object) null)
      this.forbiddenTags = ConsumerManager.instance.DefaultForbiddenTagsList.ToArray();
    else
      this.forbiddenTags = new Tag[0];
  }

  public bool IsPermitted(string consumable_id)
  {
    Tag tag = new Tag(consumable_id);
    for (int index = 0; index < this.forbiddenTags.Length; ++index)
    {
      if (this.forbiddenTags[index] == tag)
        return false;
    }
    return true;
  }

  public void SetPermitted(string consumable_id, bool is_allowed)
  {
    Tag tag = new Tag(consumable_id);
    List<Tag> tagList = new List<Tag>((IEnumerable<Tag>) this.forbiddenTags);
    if (is_allowed)
      tagList.Remove(tag);
    else if (!tagList.Contains(tag))
      tagList.Add(tag);
    this.forbiddenTags = tagList.ToArray();
    this.consumableRulesChanged.Signal();
  }
}
