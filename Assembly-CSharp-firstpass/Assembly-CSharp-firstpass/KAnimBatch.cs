// Decompiled with JetBrains decompiler
// Type: KAnimBatch
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class KAnimBatch
{
  private List<KAnimConverter.IAnimConverter> controllers = new List<KAnimConverter.IAnimConverter>();
  private Dictionary<KAnimConverter.IAnimConverter, int> controllersToIdx = new Dictionary<KAnimConverter.IAnimConverter, int>();
  private List<int> dirtySet = new List<int>();
  private static int nextBatchId;
  private int currentOffset;
  private static int ShaderProperty_SYMBOL_INSTANCE_TEXTURE_SIZE = Shader.PropertyToID("SYMBOL_INSTANCE_TEXTURE_SIZE");
  private static int ShaderProperty_symbolInstanceTex = Shader.PropertyToID(nameof (symbolInstanceTex));
  private static int ShaderProperty_SYMBOL_OVERRIDE_INFO_TEXTURE_SIZE = Shader.PropertyToID("SYMBOL_OVERRIDE_INFO_TEXTURE_SIZE");
  private static int ShaderProperty_symbolOverrideInfoTex = Shader.PropertyToID(nameof (symbolOverrideInfoTex));
  public static int ShaderProperty_SUPPORTS_SYMBOL_OVERRIDING = Shader.PropertyToID("SUPPORTS_SYMBOL_OVERRIDING");
  public static int ShaderProperty_ANIM_TEXTURE_START_OFFSET = Shader.PropertyToID("ANIM_TEXTURE_START_OFFSET");
  private KAnimBatch.SymbolInstanceSlot[] symbolInstanceSlots;
  private KAnimBatch.SymbolOverrideInfoSlot[] symbolOverrideInfoSlots;
  public KAnimBatch.AtlasList atlases = new KAnimBatch.AtlasList(0);
  private bool needsWrite;

  public int id { get; private set; }

  public bool dirty => this.dirtySet.Count > 0;

  public int dirtyCount => this.dirtySet.Count;

  public bool active { get; private set; }

  public int size => this.controllers.Count;

  public Vector3 position { get; private set; }

  public int layer { get; private set; }

  public List<KAnimConverter.IAnimConverter> Controllers => this.controllers;

  public KAnimBatchGroup.MaterialType materialType { get; private set; }

  public HashedString batchGroup { get; private set; }

  public BatchSet batchset { get; private set; }

  public KAnimBatchGroup group { get; private set; }

  public int writtenLastFrame { get; private set; }

  public MaterialPropertyBlock matProperties { get; private set; }

  public KAnimBatchGroup.KAnimBatchTextureCache.Entry dataTex { get; private set; }

  public KAnimBatchGroup.KAnimBatchTextureCache.Entry symbolInstanceTex { get; private set; }

  public KAnimBatchGroup.KAnimBatchTextureCache.Entry symbolOverrideInfoTex { get; private set; }

  public KAnimBatch(
    KAnimBatchGroup group,
    int layer,
    float z,
    KAnimBatchGroup.MaterialType material_type)
  {
    this.id = KAnimBatch.nextBatchId++;
    this.active = true;
    this.group = group;
    this.layer = layer;
    this.batchGroup = group.batchID;
    this.materialType = material_type;
    this.matProperties = new MaterialPropertyBlock();
    this.position = new Vector3(0.0f, 0.0f, z);
    this.symbolInstanceSlots = new KAnimBatch.SymbolInstanceSlot[group.maxGroupSize];
    this.symbolOverrideInfoSlots = new KAnimBatch.SymbolOverrideInfoSlot[group.maxGroupSize];
  }

  public void DestroyTex()
  {
    if (this.dataTex != null)
    {
      this.group.FreeTexture(this.dataTex);
      this.dataTex = (KAnimBatchGroup.KAnimBatchTextureCache.Entry) null;
    }
    if (this.symbolInstanceTex != null)
    {
      this.group.FreeTexture(this.symbolInstanceTex);
      this.symbolInstanceTex = (KAnimBatchGroup.KAnimBatchTextureCache.Entry) null;
    }
    if (this.symbolOverrideInfoTex == null)
      return;
    this.group.FreeTexture(this.symbolOverrideInfoTex);
    this.symbolOverrideInfoTex = (KAnimBatchGroup.KAnimBatchTextureCache.Entry) null;
  }

  public void Init()
  {
    this.dataTex = this.group.CreateTexture();
    if (this.dataTex == null)
      Debug.LogErrorFormat("Got null data texture from AnimBatchGroup [{0}]", (object) this.batchGroup);
    this.symbolInstanceTex = this.group.CreateTexture("SymbolInstanceTex", KAnimBatchGroup.GetBestTextureSize((float) (this.group.data.maxSymbolsPerBuild * this.group.maxGroupSize * 8)), KAnimBatch.ShaderProperty_symbolInstanceTex, KAnimBatch.ShaderProperty_SYMBOL_INSTANCE_TEXTURE_SIZE);
    int width = this.dataTex.width;
    if (width == 0)
    {
      Debug.LogWarning((object) ("Empty group [" + (object) this.group.batchID + "] " + (object) this.batchset.idx + " (probably just anims)"));
    }
    else
    {
      for (int index = 0; index < width * width; ++index)
      {
        this.dataTex.floats[index * 4] = -1f;
        this.dataTex.floats[index * 4 + 1] = 0.0f;
        this.dataTex.floats[index * 4 + 2] = 0.0f;
        this.dataTex.floats[index * 4 + 3] = 0.0f;
      }
      if (this.matProperties == null)
        this.matProperties = new MaterialPropertyBlock();
      this.dataTex.SetTextureAndSize(this.matProperties);
      this.symbolInstanceTex.SetTextureAndSize(this.matProperties);
      this.group.GetDataTextures(this.matProperties, this.atlases);
      this.atlases.Apply(this.matProperties);
    }
  }

  public void Clear()
  {
    this.DestroyTex();
    this.controllers.Clear();
    this.dirtySet.Clear();
    this.batchset = (BatchSet) null;
    this.group = (KAnimBatchGroup) null;
    this.matProperties = (MaterialPropertyBlock) null;
    this.dataTex = (KAnimBatchGroup.KAnimBatchTextureCache.Entry) null;
  }

  public void SetBatchSet(BatchSet newBatchSet)
  {
    if (this.batchset != null && this.batchset != newBatchSet)
      this.batchset.RemoveBatch(this);
    this.batchset = newBatchSet;
    if (this.batchset == null)
      return;
    this.position = new Vector3((float) (this.batchset.idx.x * 32), (float) (this.batchset.idx.y * 32), this.position.z);
    this.active = this.batchset.active;
  }

  public bool Register(KAnimConverter.IAnimConverter controller)
  {
    if (this.dataTex == null || this.dataTex.floats.Length == 0)
      this.Init();
    if (!this.controllers.Contains(controller))
    {
      this.controllers.Add(controller);
      this.controllersToIdx[controller] = this.controllers.Count - 1;
      this.currentOffset += 28;
    }
    this.AddToDirty(this.controllers.IndexOf(controller));
    controller.GetBatch()?.Deregister(controller);
    controller.SetBatch(this);
    return true;
  }

  public void OverrideZ(float z) => this.position = new Vector3(this.position.x, this.position.y, z);

  public void SetLayer(int layer) => this.layer = layer;

  public void Deregister(KAnimConverter.IAnimConverter controller)
  {
    if (App.IsExiting)
      return;
    if (this.controllers.IndexOf(controller) >= 0)
    {
      if (!this.controllers.Remove(controller))
        Debug.LogError((object) ("Failed to remove controller [" + controller.GetName() + "]"));
      controller.SetBatch((KAnimBatch) null);
      this.currentOffset -= 28;
      this.currentOffset = Mathf.Max(0, this.currentOffset);
      for (int index = 0; index < 28; ++index)
        this.dataTex.floats[this.currentOffset + index] = -1f;
      this.currentOffset = 28 * this.controllers.Count;
      this.ClearDirty();
      this.controllersToIdx.Clear();
      for (int index = 0; index < this.controllers.Count; ++index)
      {
        this.controllersToIdx[this.controllers[index]] = index;
        this.AddToDirty(index);
      }
    }
    else
      Debug.LogError((object) ("Deregister called for [" + controller.GetName() + "] but its not in this batch "));
    if (this.controllers.Count != 0)
      return;
    this.batchset.RemoveBatch(this);
    this.DestroyTex();
  }

  private void ClearDirty() => this.dirtySet.Clear();

  private void AddToDirty(int dirtyIdx)
  {
    if (!this.dirtySet.Contains(dirtyIdx))
      this.dirtySet.Add(dirtyIdx);
    this.batchset.SetDirty();
    this.needsWrite = true;
  }

  public void Activate() => this.active = true;

  public void Deactivate() => this.active = false;

  public void SetDirty(KAnimConverter.IAnimConverter controller)
  {
    if (!this.controllersToIdx.ContainsKey(controller))
      Debug.LogError((object) ("Setting controller [" + controller.GetName() + "] to dirty but its not in this batch"));
    else
      this.AddToDirty(this.controllersToIdx[controller]);
  }

  private void WriteBatchedAnimInstanceData(int index, KAnimConverter.IAnimConverter controller) => controller.GetBatchInstanceData().WriteToTexture(this.dataTex.bytes, index * 112, index);

  private bool WriteSymbolInstanceData(int index, KAnimConverter.IAnimConverter controller)
  {
    bool flag = false;
    KAnimBatch.SymbolInstanceSlot symbolInstanceSlot = this.symbolInstanceSlots[index];
    if (symbolInstanceSlot.symbolInstanceData != controller.symbolInstanceGpuData || symbolInstanceSlot.dataVersion != controller.symbolInstanceGpuData.version)
    {
      controller.symbolInstanceGpuData.WriteToTexture(this.symbolInstanceTex.bytes, index * 8 * this.group.data.maxSymbolsPerBuild * 4, index);
      symbolInstanceSlot.symbolInstanceData = controller.symbolInstanceGpuData;
      symbolInstanceSlot.dataVersion = controller.symbolInstanceGpuData.version;
      this.symbolInstanceSlots[index] = symbolInstanceSlot;
      flag = true;
    }
    return flag;
  }

  private bool WriteSymbolOverrideInfoTex(int index, KAnimConverter.IAnimConverter controller)
  {
    bool flag = false;
    KAnimBatch.SymbolOverrideInfoSlot overrideInfoSlot = this.symbolOverrideInfoSlots[index];
    if (overrideInfoSlot.symbolOverrideInfo != controller.symbolOverrideInfoGpuData || overrideInfoSlot.dataVersion != controller.symbolOverrideInfoGpuData.version)
    {
      controller.symbolOverrideInfoGpuData.WriteToTexture(this.symbolOverrideInfoTex.bytes, index * 12 * this.group.data.maxSymbolFrameInstancesPerbuild * 4, index);
      overrideInfoSlot.symbolOverrideInfo = controller.symbolOverrideInfoGpuData;
      overrideInfoSlot.dataVersion = controller.symbolOverrideInfoGpuData.version;
      this.symbolOverrideInfoSlots[index] = overrideInfoSlot;
      flag = true;
    }
    return flag;
  }

  public int UpdateDirty(int frame)
  {
    if (!this.needsWrite)
      return 0;
    if (this.dataTex == null || this.dataTex.floats.Length == 0)
      this.Init();
    this.writtenLastFrame = 0;
    bool flag1 = false;
    bool flag2 = false;
    if (this.dirtySet.Count > 0)
    {
      foreach (int dirty in this.dirtySet)
      {
        KAnimConverter.IAnimConverter controller = this.controllers[dirty];
        if (controller != null && controller as Object != (Object) null)
        {
          this.WriteBatchedAnimInstanceData(dirty, controller);
          bool flag3 = this.WriteSymbolInstanceData(dirty, controller);
          flag1 |= flag3;
          if (controller.ApplySymbolOverrides())
          {
            if (this.symbolOverrideInfoTex == null)
            {
              this.symbolOverrideInfoTex = this.group.CreateTexture("SymbolOverrideInfoTex", KAnimBatchGroup.GetBestTextureSize((float) (this.group.data.maxSymbolFrameInstancesPerbuild * this.group.maxGroupSize * 12)), KAnimBatch.ShaderProperty_symbolOverrideInfoTex, KAnimBatch.ShaderProperty_SYMBOL_OVERRIDE_INFO_TEXTURE_SIZE);
              this.symbolOverrideInfoTex.SetTextureAndSize(this.matProperties);
              this.matProperties.SetFloat(KAnimBatch.ShaderProperty_SUPPORTS_SYMBOL_OVERRIDING, 1f);
            }
            bool flag4 = this.WriteSymbolOverrideInfoTex(dirty, controller);
            flag2 |= flag4;
          }
          ++this.writtenLastFrame;
        }
      }
      if (this.writtenLastFrame != 0)
        this.ClearDirty();
      else
        Debug.LogError((object) "dirtySet not written");
    }
    this.dataTex.LoadRawTextureData();
    this.dataTex.Apply();
    if (flag1)
    {
      this.symbolInstanceTex.LoadRawTextureData();
      this.symbolInstanceTex.Apply();
    }
    if (flag2)
    {
      this.symbolOverrideInfoTex.LoadRawTextureData();
      this.symbolOverrideInfoTex.Apply();
    }
    return this.writtenLastFrame;
  }

  public struct SymbolInstanceSlot
  {
    public SymbolInstanceGpuData symbolInstanceData;
    public int dataVersion;
  }

  public struct SymbolOverrideInfoSlot
  {
    public SymbolOverrideInfoGpuData symbolOverrideInfo;
    public int dataVersion;
  }

  public class AtlasList
  {
    private List<Texture2D> atlases = new List<Texture2D>();
    private int startIdx;

    public AtlasList(int start_idx) => this.startIdx = start_idx;

    public int Add(Texture2D atlas)
    {
      DebugUtil.Assert((Object) atlas != (Object) null);
      DebugUtil.Assert(this.atlases.Count < KAnimBatchManager.instance.atlasNames.Length);
      int num = this.atlases.IndexOf(atlas);
      if (num == -1)
      {
        num = this.atlases.Count;
        this.atlases.Add(atlas);
      }
      return num + this.startIdx;
    }

    public void Apply(MaterialPropertyBlock material_property_block)
    {
      bool flag = false;
      for (int index1 = 0; index1 < this.atlases.Count; ++index1)
      {
        int index2 = this.startIdx + index1;
        if (index2 >= KAnimBatchManager.instance.atlasNames.Length)
          flag = true;
        else
          material_property_block.SetTexture(KAnimBatchManager.instance.atlasNames[index2], (Texture) this.atlases[index1]);
      }
      if (!flag)
        return;
      string str = "Atlas overflow: + \n";
      foreach (Texture2D atlase in this.atlases)
        str = str + atlase.name + "\n";
      Debug.LogError((object) str);
    }

    public void Clear(int start_idx)
    {
      this.atlases.Clear();
      this.startIdx = start_idx;
    }

    public int GetAtlasIdx(Texture2D atlas)
    {
      for (int index = 0; index < this.atlases.Count; ++index)
      {
        if ((Object) this.atlases[index] == (Object) atlas)
          return index + this.startIdx;
      }
      return -1;
    }

    public int Count => this.atlases.Count;
  }
}
