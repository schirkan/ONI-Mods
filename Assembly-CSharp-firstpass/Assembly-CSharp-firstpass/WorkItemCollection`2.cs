// Decompiled with JetBrains decompiler
// Type: WorkItemCollection`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public class WorkItemCollection<WorkItemType, SharedDataType> : IWorkItemCollection
  where WorkItemType : IWorkItem<SharedDataType>
{
  private List<WorkItemType> items = new List<WorkItemType>();
  private SharedDataType sharedData;

  public int Count => this.items.Count;

  public WorkItemType GetWorkItem(int idx) => this.items[idx];

  public void Add(WorkItemType work_item) => this.items.Add(work_item);

  public void InternalDoWorkItem(int work_item_idx)
  {
    WorkItemType workItemType = this.items[work_item_idx];
    workItemType.Run(this.sharedData);
    this.items[work_item_idx] = workItemType;
  }

  public void Reset(SharedDataType shared_data)
  {
    this.sharedData = shared_data;
    this.items.Clear();
  }
}
