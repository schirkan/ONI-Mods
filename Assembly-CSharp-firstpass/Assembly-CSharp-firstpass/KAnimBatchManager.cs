// Decompiled with JetBrains decompiler
// Type: KAnimBatchManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class KAnimBatchManager
{
  private const int DEFAULT_BATCH_SIZE = 30;
  public const int CHUNK_SIZE = 32;
  public static HashedString NO_BATCH = new HashedString(nameof (NO_BATCH));
  public static HashedString IGNORE = new HashedString(nameof (IGNORE));
  public static Vector2 GROUP_SIZE = new Vector2(32f, 32f);
  private bool ready;
  private Dictionary<HashedString, KBatchGroupData> batchGroupData = new Dictionary<HashedString, KBatchGroupData>();
  private Dictionary<BatchGroupKey, KAnimBatchGroup> batchGroups = new Dictionary<BatchGroupKey, KAnimBatchGroup>();
  private Dictionary<BatchKey, BatchSet> batchSets = new Dictionary<BatchKey, BatchSet>();
  private List<KAnimBatchManager.BatchSetInfo> culledBatchSetInfos = new List<KAnimBatchManager.BatchSetInfo>();
  private List<KAnimBatchManager.BatchSetInfo> uiBatchSets = new List<KAnimBatchManager.BatchSetInfo>();
  private List<BatchSet> activeBatchSets = new List<BatchSet>();
  public int[] atlasNames = new int[12]
  {
    Shader.PropertyToID("atlas0"),
    Shader.PropertyToID("atlas1"),
    Shader.PropertyToID("atlas2"),
    Shader.PropertyToID("atlas3"),
    Shader.PropertyToID("atlas4"),
    Shader.PropertyToID("atlas5"),
    Shader.PropertyToID("atlas6"),
    Shader.PropertyToID("atlas7"),
    Shader.PropertyToID("atlas8"),
    Shader.PropertyToID("atlas9"),
    Shader.PropertyToID("atlas10"),
    Shader.PropertyToID("atlas11")
  };

  public int dirtyBatchLastFrame { get; private set; }

  public static KAnimBatchManager instance => Singleton<KAnimBatchManager>.Instance;

  public static void CreateInstance() => Singleton<KAnimBatchManager>.CreateInstance();

  public static KAnimBatchManager Instance() => KAnimBatchManager.instance;

  public static void DestroyInstance()
  {
    if (KAnimBatchManager.instance != null)
    {
      KAnimBatchManager.instance.ready = false;
      foreach (KeyValuePair<BatchGroupKey, KAnimBatchGroup> batchGroup in KAnimBatchManager.instance.batchGroups)
        batchGroup.Value.FreeResources();
      KAnimBatchManager.instance.batchGroups.Clear();
      foreach (KeyValuePair<HashedString, KBatchGroupData> keyValuePair in KAnimBatchManager.instance.batchGroupData)
      {
        if (keyValuePair.Value != null)
          keyValuePair.Value.FreeResources();
      }
      KAnimBatchManager.instance.batchGroupData.Clear();
      foreach (KeyValuePair<BatchKey, BatchSet> batchSet in KAnimBatchManager.instance.batchSets)
      {
        if (batchSet.Value != null)
          batchSet.Value.Clear();
      }
      KAnimBatchManager.instance.batchSets.Clear();
      KAnimBatchManager.instance.culledBatchSetInfos.Clear();
      KAnimBatchManager.instance.uiBatchSets.Clear();
      KAnimBatchManager.instance.activeBatchSets.Clear();
      KAnimBatchManager.instance.dirtyBatchLastFrame = 0;
      KAnimBatchGroup.FinalizeTextureCache();
    }
    Singleton<KAnimBatchManager>.DestroyInstance();
  }

  public bool isReady => this.ready;

  public KBatchGroupData GetBatchGroupData(HashedString groupID)
  {
    if (!groupID.IsValid || groupID == KAnimBatchManager.NO_BATCH || groupID == KAnimBatchManager.IGNORE)
      return (KBatchGroupData) null;
    KBatchGroupData kbatchGroupData = (KBatchGroupData) null;
    if (!this.batchGroupData.TryGetValue(groupID, out kbatchGroupData))
    {
      kbatchGroupData = new KBatchGroupData(groupID);
      this.batchGroupData[groupID] = kbatchGroupData;
    }
    return kbatchGroupData;
  }

  public KAnimBatchGroup GetBatchGroup(BatchGroupKey group_key)
  {
    KAnimBatchGroup kanimBatchGroup = (KAnimBatchGroup) null;
    if (!this.batchGroups.TryGetValue(group_key, out kanimBatchGroup))
    {
      kanimBatchGroup = new KAnimBatchGroup(group_key.groupID);
      this.batchGroups.Add(group_key, kanimBatchGroup);
    }
    return kanimBatchGroup;
  }

  public static Vector2I CellXYToChunkXY(Vector2I cell_xy) => new Vector2I(cell_xy.x / 32, cell_xy.y / 32);

  public static Vector2I ControllerToChunkXY(KAnimConverter.IAnimConverter controller) => KAnimBatchManager.CellXYToChunkXY(controller.GetCellXY());

  public void Register(KAnimConverter.IAnimConverter controller)
  {
    if (!this.isReady)
      Debug.LogError((object) string.Format("Batcher isnt finished setting up, controller [{0}] is registering too early.", (object) controller.GetName()));
    BatchKey batchKey = BatchKey.Create(controller);
    Vector2I chunkXy = KAnimBatchManager.ControllerToChunkXY(controller);
    BatchSet batchSet;
    if (!this.batchSets.TryGetValue(batchKey, out batchSet))
    {
      batchSet = new BatchSet(this.GetBatchGroup(new BatchGroupKey(batchKey.groupID)), batchKey, chunkXy);
      this.batchSets[batchKey] = batchSet;
      if (batchSet.key.materialType == KAnimBatchGroup.MaterialType.UI)
        this.uiBatchSets.Add(new KAnimBatchManager.BatchSetInfo()
        {
          batchSet = batchSet,
          isActive = false,
          spatialIdx = chunkXy
        });
      else
        this.culledBatchSetInfos.Add(new KAnimBatchManager.BatchSetInfo()
        {
          batchSet = batchSet,
          isActive = false,
          spatialIdx = chunkXy
        });
    }
    batchSet.Add(controller);
  }

  public void UpdateActiveArea(Vector2I vis_chunk_min, Vector2I vis_chunk_max)
  {
    this.activeBatchSets.Clear();
    for (int index = 0; index < this.uiBatchSets.Count; ++index)
    {
      KAnimBatchManager.BatchSetInfo uiBatchSet = this.uiBatchSets[index];
      this.activeBatchSets.Add(uiBatchSet.batchSet);
      if (!uiBatchSet.isActive)
      {
        uiBatchSet.isActive = true;
        uiBatchSet.batchSet.SetActive(true);
        this.uiBatchSets[index] = uiBatchSet;
      }
    }
    for (int index = 0; index < this.culledBatchSetInfos.Count; ++index)
    {
      KAnimBatchManager.BatchSetInfo culledBatchSetInfo = this.culledBatchSetInfos[index];
      if (culledBatchSetInfo.spatialIdx.x >= vis_chunk_min.x && culledBatchSetInfo.spatialIdx.x <= vis_chunk_max.x && (culledBatchSetInfo.spatialIdx.y >= vis_chunk_min.y && culledBatchSetInfo.spatialIdx.y <= vis_chunk_max.y))
      {
        this.activeBatchSets.Add(culledBatchSetInfo.batchSet);
        if (!culledBatchSetInfo.isActive)
        {
          culledBatchSetInfo.isActive = true;
          this.culledBatchSetInfos[index] = culledBatchSetInfo;
          culledBatchSetInfo.batchSet.SetActive(true);
        }
      }
      else if (culledBatchSetInfo.isActive)
      {
        culledBatchSetInfo.isActive = false;
        this.culledBatchSetInfos[index] = culledBatchSetInfo;
        culledBatchSetInfo.batchSet.SetActive(false);
      }
    }
  }

  public int UpdateDirty(int frame)
  {
    if (!this.ready)
      return 0;
    this.dirtyBatchLastFrame = 0;
    foreach (BatchSet activeBatchSet in this.activeBatchSets)
      this.dirtyBatchLastFrame += activeBatchSet.UpdateDirty(frame);
    return this.dirtyBatchLastFrame;
  }

  public void Render()
  {
    if (!this.ready)
      return;
    foreach (BatchSet activeBatchSet in this.activeBatchSets)
    {
      DebugUtil.Assert(activeBatchSet != null);
      DebugUtil.Assert(activeBatchSet.group != null);
      Mesh mesh = activeBatchSet.group.mesh;
      for (int idx = 0; idx < activeBatchSet.batchCount; ++idx)
      {
        KAnimBatch batch = activeBatchSet.GetBatch(idx);
        float num = 0.01f / (float) (1 + batch.id % 256);
        if (batch.size != 0 && batch.active && batch.materialType != KAnimBatchGroup.MaterialType.UI)
        {
          Vector3 zero = Vector3.zero;
          zero.z = batch.position.z + num;
          int layer = batch.layer;
          Graphics.DrawMesh(mesh, zero, Quaternion.identity, activeBatchSet.group.GetMaterial(batch.materialType), layer, (Camera) null, 0, batch.matProperties);
        }
      }
    }
  }

  public void CompleteInit() => this.ready = true;

  private struct BatchSetInfo
  {
    public BatchSet batchSet;
    public Vector2I spatialIdx;
    public bool isActive;
  }
}
