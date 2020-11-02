// Decompiled with JetBrains decompiler
// Type: KAnimBatchGroup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

public class KAnimBatchGroup
{
  public static int ShaderProperty_SYMBOLS_PER_BUILD = Shader.PropertyToID("SYMBOLS_PER_BUILD");
  public static int ShaderProperty_ANIM_TEXTURE_START_OFFSET = Shader.PropertyToID("ANIM_TEXTURE_START_OFFSET");
  public static int ShaderProperty_SYMBOL_OVERRIDES_PER_BUILD = Shader.PropertyToID("SYMBOL_OVERRIDES_PER_BUILD");
  private static Color ResetColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  private static KAnimBatchGroup.KAnimBatchTextureCache cache = new KAnimBatchGroup.KAnimBatchTextureCache();
  public int batchCount;
  private int float4sPerSide;
  private static int ShaderProperty_BUILD_AND_ANIM_TEXTURE_SIZE = Shader.PropertyToID("BUILD_AND_ANIM_TEXTURE_SIZE");
  private static int ShaderProperty_buildAndAnimTex = Shader.PropertyToID(nameof (buildAndAnimTex));
  private static int ShaderProperty_INSTANCE_TEXTURE_SIZE = Shader.PropertyToID("INSTANCE_TEXTURE_SIZE");
  private static int ShaderProperty_instanceTex = Shader.PropertyToID("instanceTex");
  private Material[] materials;

  public static void FinalizeTextureCache() => KAnimBatchGroup.cache.Finalise();

  private Material CreateMaterial(KAnimBatchGroup.MaterialType material_type)
  {
    Material material;
    switch (material_type)
    {
      case KAnimBatchGroup.MaterialType.Simple:
        material = new Material(Shader.Find("Klei/AnimationSimple"));
        break;
      case KAnimBatchGroup.MaterialType.UI:
        material = new Material(Shader.Find("Klei/BatchedAnimationUI"));
        break;
      case KAnimBatchGroup.MaterialType.Overlay:
        material = new Material(Shader.Find("Klei/AnimationOverlay"));
        break;
      default:
        material = new Material(Shader.Find("Klei/BatchedAnimation"));
        break;
    }
    material.name = "Material:" + this.batchID.ToString();
    material.SetFloat(KAnimBatchGroup.ShaderProperty_SYMBOLS_PER_BUILD, (float) this.data.maxSymbolsPerBuild);
    material.SetFloat(KAnimBatchGroup.ShaderProperty_ANIM_TEXTURE_START_OFFSET, (float) this.data.animDataStartOffset);
    material.SetFloat(KAnimBatchGroup.ShaderProperty_SYMBOL_OVERRIDES_PER_BUILD, (float) this.data.symbolFrameInstances.Count);
    return material;
  }

  public Material GetMaterial(KAnimBatchGroup.MaterialType material_type)
  {
    int index = (int) material_type;
    if ((Object) this.materials[index] == (Object) null)
      this.materials[index] = this.CreateMaterial(material_type);
    return this.materials[index];
  }

  public int maxGroupSize { get; private set; }

  public Mesh mesh { get; private set; }

  public HashedString batchID { get; private set; }

  public KBatchGroupData data { get; private set; }

  public KAnimBatchGroup.KAnimBatchTextureCache.Entry buildAndAnimTex { get; private set; }

  public KAnimBatchGroup(HashedString id)
  {
    this.data = KAnimBatchManager.Instance().GetBatchGroupData(id);
    this.materials = new Material[5];
    this.batchID = id;
    KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(id);
    if (group == null)
      return;
    this.maxGroupSize = group.maxGroupSize;
    if (this.maxGroupSize <= 0)
      this.maxGroupSize = 30;
    this.SetupMeshData();
    this.InitBuildAndAnimTex();
  }

  public bool InitOK => this.float4sPerSide > 0;

  public void FreeResources()
  {
    if (this.buildAndAnimTex != null)
    {
      KAnimBatchGroup.cache.Free(this.buildAndAnimTex);
      this.buildAndAnimTex = (KAnimBatchGroup.KAnimBatchTextureCache.Entry) null;
    }
    for (int index = 0; index < 5; ++index)
    {
      if ((Object) this.materials[index] != (Object) null)
      {
        Object.Destroy((Object) this.materials[index]);
        this.materials[index] = (Material) null;
      }
    }
    if ((Object) this.mesh != (Object) null)
    {
      Object.Destroy((Object) this.mesh);
      this.mesh = (Mesh) null;
    }
    if (this.data != null)
      this.data.FreeResources();
    this.data = (KBatchGroupData) null;
  }

  public static int GetBestTextureSize(float cost)
  {
    int num1 = Mathf.CeilToInt(Mathf.Sqrt(cost));
    int num2 = 32;
    return Mathf.CeilToInt((float) num1 / (float) num2) * num2;
  }

  private void SetupMeshData()
  {
    Debug.Assert(this.maxGroupSize > 0, (object) "Group size must be >0");
    this.maxGroupSize = Mathf.Min(this.maxGroupSize, 30);
    this.mesh = this.BuildMesh(this.maxGroupSize * this.data.maxVisibleSymbols);
    this.float4sPerSide = KAnimBatchGroup.GetBestTextureSize((float) (this.maxGroupSize * 28) / 4f);
  }

  private float GetBuildDataSize() => (float) (this.data.GetBuildSymbolFrameCount() * 16) / 4f;

  private float GetAnimDataSize()
  {
    int num = 4;
    List<KAnim.Anim.Frame> animFrames = this.data.GetAnimFrames();
    return (animFrames.Count != 0 ? (float) (num + animFrames.Count * 4 + this.data.GetAnimFrameElements().Count * 16) : (float) (num + this.data.symbolFrameInstances.Count * 4 + this.data.symbolFrameInstances.Count * 16)) / 4f;
  }

  public void InitBuildAndAnimTex()
  {
    float cost = this.GetBuildDataSize() + this.GetAnimDataSize();
    int bestTextureSize = KAnimBatchGroup.GetBestTextureSize(cost);
    this.buildAndAnimTex = KAnimBatchGroup.cache.Get(bestTextureSize, KAnimBatchGroup.ShaderProperty_buildAndAnimTex, KAnimBatchGroup.ShaderProperty_BUILD_AND_ANIM_TEXTURE_SIZE);
    this.buildAndAnimTex.name = "BuildAndAnimData:" + this.batchID.ToString();
    if ((double) cost > (double) (this.buildAndAnimTex.width * this.buildAndAnimTex.height))
      Debug.LogErrorFormat("Texture is the wrong size! {0} <= {1}", (object) cost, (object) (this.buildAndAnimTex.width * this.buildAndAnimTex.height));
    this.data.WriteAnimData(this.data.WriteBuildData(this.data.symbolFrameInstances, this.buildAndAnimTex.floats), this.buildAndAnimTex.floats);
    this.buildAndAnimTex.LoadRawTextureData();
    this.buildAndAnimTex.Apply();
  }

  private Mesh BuildMesh(int numQuads)
  {
    Mesh mesh = new Mesh();
    int[] indices = new int[numQuads * 6];
    for (int index1 = 0; index1 < numQuads; ++index1)
    {
      int index2 = index1 * 6;
      int num = index1 * 4;
      indices[index2] = num;
      indices[index2 + 1] = num + 1;
      indices[index2 + 2] = num + 2;
      indices[index2 + 3] = num + 1;
      indices[index2 + 4] = num + 2;
      indices[index2 + 5] = num + 3;
    }
    Vector3[] vector3Array = new Vector3[numQuads * 4];
    Vector2[] vector2Array = new Vector2[numQuads * 4];
    Vector4[] vector4Array = new Vector4[numQuads * 4];
    for (int index1 = 0; index1 < numQuads; ++index1)
    {
      int index2 = index1 * 4;
      vector3Array[index2] = Vector3.zero;
      vector3Array[index2 + 1] = Vector3.zero;
      vector3Array[index2 + 2] = Vector3.zero;
      vector3Array[index2 + 3] = Vector3.zero;
      Vector2 vector2 = new Vector2((float) (index1 / this.data.maxVisibleSymbols), (float) (this.data.maxVisibleSymbols - index1 % this.data.maxVisibleSymbols - 1));
      vector2Array[index2] = vector2;
      vector2Array[index2 + 1] = vector2;
      vector2Array[index2 + 2] = vector2;
      vector2Array[index2 + 3] = vector2;
      vector4Array[index2] = new Vector4(0.0f, (float) index2, (float) index1);
      vector4Array[index2 + 1] = new Vector4(1f, (float) index2, (float) index1);
      vector4Array[index2 + 2] = new Vector4(2f, (float) index2, (float) index1);
      vector4Array[index2 + 3] = new Vector4(3f, (float) index2, (float) index1);
    }
    mesh.name = "BatchGroup:" + this.batchID.ToString();
    mesh.vertices = vector3Array;
    mesh.SetUVs(0, new List<Vector2>((IEnumerable<Vector2>) vector2Array));
    mesh.SetUVs(1, new List<Vector4>((IEnumerable<Vector4>) vector4Array));
    mesh.SetIndices(indices, MeshTopology.Triangles, 0);
    mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
    return mesh;
  }

  public KAnimBatchGroup.KAnimBatchTextureCache.Entry CreateTexture(
    string name,
    int size_in_floats,
    int texture_property_id,
    int texture_size_property_id)
  {
    DebugUtil.Assert(size_in_floats > 0);
    KAnimBatchGroup.KAnimBatchTextureCache.Entry entry = KAnimBatchGroup.cache.Get(size_in_floats, texture_property_id, texture_size_property_id);
    entry.name = name;
    return entry;
  }

  public KAnimBatchGroup.KAnimBatchTextureCache.Entry CreateTexture()
  {
    if (this.float4sPerSide <= 0)
      Debug.LogErrorFormat("Need to init AnimBatchGroup [{0}] first!", (object) this.batchID);
    return this.CreateTexture("InstanceData:" + this.batchID.ToString(), this.float4sPerSide, KAnimBatchGroup.ShaderProperty_instanceTex, KAnimBatchGroup.ShaderProperty_INSTANCE_TEXTURE_SIZE);
  }

  public void FreeTexture(KAnimBatchGroup.KAnimBatchTextureCache.Entry entry) => KAnimBatchGroup.cache.Free(entry);

  public void GetDataTextures(MaterialPropertyBlock matProperties, KAnimBatch.AtlasList atlases)
  {
    if (this.buildAndAnimTex != null)
      this.buildAndAnimTex.SetTextureAndSize(matProperties);
    matProperties.SetFloat(KAnimBatchGroup.ShaderProperty_ANIM_TEXTURE_START_OFFSET, (float) this.data.animDataStartOffset);
    for (int index = 0; index < this.data.textures.Count; ++index)
      atlases.Add(this.data.textures[index]);
  }

  public class KAnimBatchTextureCache
  {
    private Dictionary<int, List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>> unused = new Dictionary<int, List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>>();
    private Dictionary<int, List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>> inuse = new Dictionary<int, List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>>();

    public KAnimBatchGroup.KAnimBatchTextureCache.Entry Get(
      int float4s_per_side,
      int texture_property_id,
      int texture_size_property_id)
    {
      List<KAnimBatchGroup.KAnimBatchTextureCache.Entry> entryList1 = (List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>) null;
      if (!this.unused.TryGetValue(float4s_per_side, out entryList1))
      {
        entryList1 = new List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>();
        this.unused.Add(float4s_per_side, entryList1);
      }
      KAnimBatchGroup.KAnimBatchTextureCache.Entry entry;
      if (entryList1.Count > 0)
      {
        int index = entryList1.Count - 1;
        entry = entryList1[index];
        entryList1.RemoveAt(index);
      }
      else
        entry = new KAnimBatchGroup.KAnimBatchTextureCache.Entry(float4s_per_side);
      entry.texturePropertyId = texture_property_id;
      entry.textureSizePropertyId = texture_size_property_id;
      List<KAnimBatchGroup.KAnimBatchTextureCache.Entry> entryList2 = (List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>) null;
      if (!this.inuse.TryGetValue(float4s_per_side, out entryList2))
      {
        entryList2 = new List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>();
        this.inuse.Add(float4s_per_side, entryList2);
      }
      entryList2.Add(entry);
      entry.cacheIndex = entryList2.Count - 1;
      return entry;
    }

    public void Free(KAnimBatchGroup.KAnimBatchTextureCache.Entry entry)
    {
      int width = entry.texture.width;
      int cacheIndex = entry.cacheIndex;
      entry.cacheIndex = -1;
      List<KAnimBatchGroup.KAnimBatchTextureCache.Entry> entryList1 = (List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>) null;
      if (this.inuse.TryGetValue(width, out entryList1))
      {
        int index = entryList1.Count - 1;
        if (index != cacheIndex)
        {
          KAnimBatchGroup.KAnimBatchTextureCache.Entry entry1 = entryList1[index];
          entry1.cacheIndex = cacheIndex;
          entryList1[cacheIndex] = entry1;
          entryList1[index] = (KAnimBatchGroup.KAnimBatchTextureCache.Entry) null;
          entryList1.RemoveAt(index);
        }
      }
      List<KAnimBatchGroup.KAnimBatchTextureCache.Entry> entryList2 = (List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>) null;
      if (!this.unused.TryGetValue(width, out entryList2))
        return;
      entryList2.Add(entry);
    }

    public void Finalise()
    {
      foreach (KeyValuePair<int, List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>> keyValuePair in this.inuse)
      {
        for (int index = 0; index < keyValuePair.Value.Count; ++index)
          Object.Destroy((Object) keyValuePair.Value[index].texture);
      }
      this.inuse.Clear();
      foreach (KeyValuePair<int, List<KAnimBatchGroup.KAnimBatchTextureCache.Entry>> keyValuePair in this.unused)
      {
        for (int index = 0; index < keyValuePair.Value.Count; ++index)
          Object.Destroy((Object) keyValuePair.Value[index].texture);
      }
      this.unused.Clear();
    }

    public class Entry
    {
      private KAnimBatchGroup.KAnimBatchTextureCache.Entry.ByteToFloatConverter floatConverter;
      public int texturePropertyId;
      public int textureSizePropertyId;
      public int cacheIndex = -1;

      public Texture2D texture { get; private set; }

      public byte[] bytes => this.floatConverter.bytes;

      public float[] floats => this.floatConverter.floats;

      public Entry(int float4s_per_side)
      {
        this.texture = new Texture2D(float4s_per_side, float4s_per_side, TextureFormat.RGBAFloat, false);
        this.texture.wrapMode = TextureWrapMode.Clamp;
        this.texture.filterMode = FilterMode.Point;
        this.texture.anisoLevel = 0;
        this.floatConverter = new KAnimBatchGroup.KAnimBatchTextureCache.Entry.ByteToFloatConverter()
        {
          bytes = new byte[float4s_per_side * float4s_per_side * 4 * 4]
        };
        int num = float4s_per_side * float4s_per_side;
        NativeArray<Color> rawTextureData = this.texture.GetRawTextureData<Color>();
        for (int index = 0; index < num; ++index)
          rawTextureData[index] = KAnimBatchGroup.ResetColor;
      }

      public void SetTextureAndSize(MaterialPropertyBlock property_block)
      {
        property_block.SetTexture(this.texturePropertyId, (Texture) this.texture);
        property_block.SetVector(this.textureSizePropertyId, new Vector4(this.texelSize.x, this.texelSize.y, (float) this.width, (float) this.height));
      }

      public void Apply() => this.texture.Apply();

      public void LoadRawTextureData() => this.texture.LoadRawTextureData(this.bytes);

      public int width => this.texture.width;

      public int height => this.texture.height;

      public Vector2 texelSize => this.texture.texelSize;

      public string name
      {
        get => this.texture.name;
        set => this.texture.name = value;
      }

      [StructLayout(LayoutKind.Explicit)]
      public struct ByteToFloatConverter
      {
        [FieldOffset(0)]
        public byte[] bytes;
        [FieldOffset(0)]
        public float[] floats;
      }
    }
  }

  public enum RendererType
  {
    Default,
    UI,
    StaticBatch,
    DontRender,
    AnimOnly,
  }

  public enum MaterialType
  {
    Default,
    Simple,
    Placer,
    UI,
    Overlay,
    NumMaterials,
  }
}
