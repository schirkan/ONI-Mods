﻿// Decompiled with JetBrains decompiler
// Type: ChoreType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("{IdHash}")]
public class ChoreType : Resource
{
  public StatusItem statusItem;
  public HashSet<Tag> tags = new HashSet<Tag>();
  public HashSet<Tag> interruptExclusion;
  public string reportName;

  public Urge urge { get; private set; }

  public ChoreGroup[] groups { get; private set; }

  public int priority { get; private set; }

  public int interruptPriority { get; set; }

  public int explicitPriority { get; private set; }

  private string ResolveStringCallback(string str, object data) => ((Chore) data).ResolveString(str);

  public ChoreType(
    string id,
    ResourceSet parent,
    string[] chore_groups,
    string urge,
    string name,
    string status_message,
    string tooltip,
    IEnumerable<Tag> interrupt_exclusion,
    int implicit_priority,
    int explicit_priority)
    : base(id, parent, name)
  {
    this.statusItem = new StatusItem(id, status_message, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveStringCallback);
    this.tags.Add(TagManager.Create(id));
    this.interruptExclusion = new HashSet<Tag>(interrupt_exclusion);
    Db.Get().DuplicantStatusItems.Add(this.statusItem);
    this.groups = new ChoreGroup[chore_groups.Length];
    for (int index = 0; index < this.groups.Length; ++index)
    {
      ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(chore_groups[index]);
      if (!choreGroup.choreTypes.Contains(this))
        choreGroup.choreTypes.Add(this);
      this.groups[index] = choreGroup;
    }
    if (!string.IsNullOrEmpty(urge))
      this.urge = Db.Get().Urges.Get(urge);
    this.priority = implicit_priority;
    this.explicitPriority = explicit_priority;
  }
}