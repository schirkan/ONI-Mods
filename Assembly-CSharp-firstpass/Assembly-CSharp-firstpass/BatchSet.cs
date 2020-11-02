// Decompiled with JetBrains decompiler
// Type: BatchSet
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public class BatchSet
{
  public KAnimBatchGroup group { get; private set; }

  protected List<KAnimBatch> batches { get; private set; }

  public Vector2I idx { get; private set; }

  public BatchKey key { get; private set; }

  public bool dirty { get; private set; }

  public bool active { get; private set; }

  public int batchCount => this.batches.Count;

  public int dirtyBatchLastFrame { get; private set; }

  public BatchSet(KAnimBatchGroup batchGroup, BatchKey batchKey, Vector2I spacialIdx)
  {
    this.idx = spacialIdx;
    this.key = batchKey;
    this.dirty = true;
    this.group = batchGroup;
    this.batches = new List<KAnimBatch>();
  }

  public void Clear()
  {
    this.group = (KAnimBatchGroup) null;
    for (int index = 0; index < this.batches.Count; ++index)
    {
      if (this.batches[index] != null)
        this.batches[index].Clear();
    }
    this.batches.Clear();
  }

  public KAnimBatch GetBatch(int idx) => this.batches[idx];

  public void Add(KAnimConverter.IAnimConverter controller)
  {
    int layer = controller.GetLayer();
    if (layer != this.key.layer)
      Debug.LogError((object) ("Registering with wrong batch set (layer) " + controller.GetName()));
    if (!(controller.GetBatchGroupID() == this.key.groupID))
      Debug.LogError((object) ("Registering with wrong batch set (groupID) " + controller.GetName()));
    KAnimBatchGroup.MaterialType materialType = controller.GetMaterialType();
    for (int index = 0; index < this.batches.Count; ++index)
    {
      if (this.batches[index].size < this.group.maxGroupSize && this.batches[index].materialType == materialType)
      {
        if (!this.batches[index].Register(controller))
          return;
        this.SetDirty();
        return;
      }
    }
    KAnimBatch batch = new KAnimBatch(this.group, layer, controller.GetZ(), materialType);
    batch.Init();
    this.AddBatch(batch);
    batch.Register(controller);
  }

  public void RemoveBatch(KAnimBatch batch)
  {
    Debug.Assert(batch.batchset == this);
    if (!this.batches.Contains(batch))
      return;
    --this.group.batchCount;
    this.batches.Remove(batch);
    batch.SetBatchSet((BatchSet) null);
  }

  public void AddBatch(KAnimBatch batch)
  {
    if (batch.batchset != this)
    {
      if (batch.batchset != null)
        batch.batchset.RemoveBatch(batch);
      batch.SetBatchSet(this);
      if (!this.batches.Contains(batch))
      {
        ++this.group.batchCount;
        this.batches.Add(batch);
        this.batches.Sort((Comparison<KAnimBatch>) ((b0, b1) => b0.position.z.CompareTo(b1.position.z)));
      }
    }
    Debug.Assert((double) batch.position.x == (double) (this.idx.x * 32));
    Debug.Assert((double) batch.position.y == (double) (this.idx.y * 32));
    this.SetDirty();
  }

  public void SetDirty() => this.dirty = true;

  public void SetActive(bool isActive)
  {
    if (isActive != this.active)
    {
      if (!isActive)
      {
        for (int index = 0; index < this.batches.Count; ++index)
        {
          if (this.batches[index] != null)
            this.batches[index].Deactivate();
        }
      }
      else
      {
        for (int index = 0; index < this.batches.Count; ++index)
        {
          if (this.batches[index] != null)
            this.batches[index].Activate();
        }
        this.SetDirty();
      }
    }
    this.active = isActive;
  }

  public int lastDirtyFrame { get; private set; }

  public int UpdateDirty(int frame)
  {
    this.dirtyBatchLastFrame = 0;
    if (this.dirty)
    {
      for (int index = 0; index < this.batches.Count; ++index)
        this.dirtyBatchLastFrame += this.batches[index].UpdateDirty(frame);
      this.lastDirtyFrame = frame;
      this.dirty = false;
    }
    return this.dirtyBatchLastFrame;
  }
}
