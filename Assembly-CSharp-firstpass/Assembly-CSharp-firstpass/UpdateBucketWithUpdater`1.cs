// Decompiled with JetBrains decompiler
// Type: UpdateBucketWithUpdater`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("{name}")]
public class UpdateBucketWithUpdater<DataType> : StateMachineUpdater.BaseUpdateBucket
{
  private KCompactedVector<UpdateBucketWithUpdater<DataType>.Entry> entries = new KCompactedVector<UpdateBucketWithUpdater<DataType>.Entry>();
  private List<HandleVector<int>.Handle> pendingRemovals = new List<HandleVector<int>.Handle>();
  public UpdateBucketWithUpdater<DataType>.BatchUpdateDelegate batch_update_delegate;

  public override int count => this.entries.Count;

  public UpdateBucketWithUpdater(string name)
    : base(name)
  {
  }

  public HandleVector<int>.Handle Add(
    DataType data,
    float last_update_time,
    UpdateBucketWithUpdater<DataType>.IUpdater updater)
  {
    UpdateBucketWithUpdater<DataType>.Entry entry = new UpdateBucketWithUpdater<DataType>.Entry();
    entry.data = data;
    entry.lastUpdateTime = last_update_time;
    entry.updater = updater;
    HandleVector<int>.Handle handle = this.entries.Allocate(entry);
    this.entries.SetData(handle, entry);
    return handle;
  }

  public override void Remove(HandleVector<int>.Handle handle)
  {
    this.pendingRemovals.Add(handle);
    UpdateBucketWithUpdater<DataType>.Entry data = this.entries.GetData(handle);
    data.updater = (UpdateBucketWithUpdater<DataType>.IUpdater) null;
    this.entries.SetData(handle, data);
  }

  public override void Update(float dt)
  {
    List<UpdateBucketWithUpdater<DataType>.Entry> dataList = this.entries.GetDataList();
    foreach (HandleVector<int>.Handle pendingRemoval in this.pendingRemovals)
      this.entries.Free(pendingRemoval);
    this.pendingRemovals.Clear();
    if (this.batch_update_delegate != null)
    {
      this.batch_update_delegate(dataList, dt);
    }
    else
    {
      int count = dataList.Count;
      for (int index = 0; index < count; ++index)
      {
        UpdateBucketWithUpdater<DataType>.Entry entry = dataList[index];
        if (entry.updater != null)
        {
          entry.updater.Update(entry.data, dt - entry.lastUpdateTime);
          entry.lastUpdateTime = 0.0f;
          dataList[index] = entry;
        }
      }
    }
  }

  public struct Entry
  {
    public DataType data;
    public float lastUpdateTime;
    public UpdateBucketWithUpdater<DataType>.IUpdater updater;
  }

  public interface IUpdater
  {
    void Update(DataType smi, float dt);
  }

  public delegate void BatchUpdateDelegate(
    List<UpdateBucketWithUpdater<DataType>.Entry> items,
    float time_delta);
}
