// Decompiled with JetBrains decompiler
// Type: KAnimFile
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class KAnimFile : ScriptableObject
{
  public const string ANIM_ROOT_PATH = "Assets/anim";
  [SerializeField]
  private TextAsset animFile;
  [SerializeField]
  private TextAsset buildFile;
  [SerializeField]
  private List<Texture2D> textures = new List<Texture2D>();
  public KAnimFile.Mod mod;
  private KAnimFileData data;
  private HashedString _batchTag;
  public string homedirectory = "";

  public byte[] animBytes
  {
    get
    {
      if (this.mod != null)
        return this.mod.anim;
      return !((Object) this.animFile != (Object) null) ? (byte[]) null : this.animFile.bytes;
    }
  }

  public byte[] buildBytes
  {
    get
    {
      if (this.mod != null)
        return this.mod.build;
      return !((Object) this.buildFile != (Object) null) ? (byte[]) null : this.buildFile.bytes;
    }
  }

  public List<Texture2D> textureList => this.mod != null ? this.mod.textures : this.textures;

  public void Initialize(TextAsset anim, TextAsset build, IList<Texture2D> textures)
  {
    this.animFile = anim;
    this.buildFile = build;
    this.textures.Clear();
    this.textures.AddRange((IEnumerable<Texture2D>) textures);
  }

  public HashedString batchTag
  {
    get
    {
      if (this._batchTag.IsValid)
        return this._batchTag;
      if (this.homedirectory == null || this.homedirectory == "")
        return KAnimBatchManager.NO_BATCH;
      this._batchTag = KAnimGroupFile.GetGroupFile().GetGroupForHomeDirectory(new HashedString(this.homedirectory));
      return this._batchTag;
    }
  }

  public KAnimFileData GetData()
  {
    if (this.data == null)
    {
      KGlobalAnimParser kglobalAnimParser = KGlobalAnimParser.Get();
      if (kglobalAnimParser != null)
        this.data = kglobalAnimParser.Load(this);
    }
    return this.data;
  }

  public class Mod
  {
    public byte[] anim;
    public byte[] build;
    public List<Texture2D> textures = new List<Texture2D>();

    public bool IsValid() => this.anim != null;
  }
}
