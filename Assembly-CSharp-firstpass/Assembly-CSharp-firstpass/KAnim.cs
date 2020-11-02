// Decompiled with JetBrains decompiler
// Type: KAnim
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using UnityEngine;

public class KAnim
{
  public enum PlayMode
  {
    Loop,
    Once,
    Paused,
  }

  public enum LayerFlags
  {
    FG = 1,
  }

  public enum SymbolFlags
  {
    Bloom = 1,
    OnLight = 2,
    SnapTo = 4,
    FG = 8,
  }

  [Serializable]
  public struct AnimHashTable
  {
    public KAnimHashedString[] hashes;
  }

  [DebuggerDisplay("{id} {animFile}")]
  [Serializable]
  public class Anim
  {
    public string name;
    public HashedString id;
    public float frameRate;
    public int firstFrameIdx;
    public int numFrames;
    public HashedString rootSymbol;
    public HashedString hash;
    public float totalTime;
    public float scaledBoundingRadius;
    public Vector2 unScaledSize = Vector2.zero;

    public int index { get; private set; }

    public KAnimFileData animFile { get; private set; }

    public Anim(KAnimFileData anim_file, int idx)
    {
      this.animFile = anim_file;
      this.index = idx;
    }

    public int GetFrameIdx(KAnim.PlayMode mode, float t)
    {
      if (this.numFrames <= 0)
        return -1;
      int num = 0;
      if (mode != KAnim.PlayMode.Loop)
      {
        if (mode == KAnim.PlayMode.Once)
          ;
      }
      else
        t %= this.totalTime;
      if ((double) t > 0.0)
        num = Math.Min(this.numFrames - 1, (int) (float) ((double) t * (double) this.frameRate + 0.499999970197678));
      return num;
    }

    private static KBatchGroupData GetAnimBatchGroupData(KAnimFileData animFile)
    {
      if (!animFile.batchTag.IsValid)
        Debug.LogErrorFormat("Invalid batchTag for anim [{0}]", (object) animFile.name);
      Debug.Assert(animFile.batchTag.IsValid, (object) "Invalid batch tag");
      KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(animFile.batchTag);
      if (group == null)
        Debug.LogErrorFormat("Null group for tag [{0}]", (object) animFile.batchTag);
      HashedString groupID = animFile.batchTag;
      if (group.renderType == KAnimBatchGroup.RendererType.DontRender || group.renderType == KAnimBatchGroup.RendererType.AnimOnly)
      {
        if (!group.swapTarget.IsValid)
          Debug.LogErrorFormat("Invalid swap target for group [{0}]", (object) group.id);
        groupID = group.swapTarget;
      }
      KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(groupID);
      if (batchGroupData == null)
        Debug.LogErrorFormat("Null batch group for tag [{0}]", (object) groupID);
      return batchGroupData;
    }

    public KAnim.Anim.Frame GetFrame(KAnimFileData animFile, KAnim.PlayMode mode, float t)
    {
      int frameIdx = this.GetFrameIdx(mode, t);
      return frameIdx < 0 || !animFile.batchTag.IsValid || !(animFile.batchTag != KAnimBatchManager.NO_BATCH) ? KAnim.Anim.Frame.InvalidFrame : KAnim.Anim.GetAnimBatchGroupData(animFile).GetFrame(this.firstFrameIdx + frameIdx);
    }

    public KAnim.Anim.Frame GetFrame(HashedString batchTag, int idx) => KAnimBatchManager.Instance().GetBatchGroupData(batchTag).GetFrame(idx + this.firstFrameIdx);

    public KAnim.Anim Copy() => new KAnim.Anim(this.animFile, this.index)
    {
      name = this.name,
      id = this.id,
      hash = this.hash,
      rootSymbol = this.rootSymbol,
      frameRate = this.frameRate,
      firstFrameIdx = this.firstFrameIdx,
      numFrames = this.numFrames,
      totalTime = this.totalTime,
      scaledBoundingRadius = this.scaledBoundingRadius,
      unScaledSize = this.unScaledSize
    };

    [Serializable]
    public struct Frame
    {
      public AABB3 bbox;
      public int firstElementIdx;
      public int idx;
      public int numElements;
      public static readonly KAnim.Anim.Frame InvalidFrame = new KAnim.Anim.Frame()
      {
        idx = -1
      };

      public bool IsValid() => this.idx != -1;

      public static bool operator ==(KAnim.Anim.Frame a, KAnim.Anim.Frame b) => a.idx == b.idx;

      public static bool operator !=(KAnim.Anim.Frame a, KAnim.Anim.Frame b) => a.idx != b.idx;

      public override bool Equals(object obj) => this.idx == ((KAnim.Anim.Frame) obj).idx;

      public override int GetHashCode() => this.idx;
    }

    [Serializable]
    public struct FrameElement
    {
      public KAnimHashedString fileHash;
      public KAnimHashedString symbol;
      public int symbolIdx;
      public KAnimHashedString folder;
      public int frame;
      public Matrix2x3 transform;
      public Color multColour;
      public int flags;
    }
  }

  [Serializable]
  public class Build : ISerializationCallbackReceiver
  {
    public KAnimHashedString fileHash;
    public int index;
    public string name;
    public HashedString batchTag;
    public int textureStartIdx;
    public int textureCount;
    public KAnim.Build.Symbol[] symbols;
    public KAnim.Build.SymbolFrame[] frames;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
      if (this.symbols == null)
        return;
      for (int index = 0; index < this.symbols.Length; ++index)
        this.symbols[index].build = this;
    }

    public KAnim.Build.Symbol GetSymbolByIndex(uint index) => (long) index >= (long) this.symbols.Length ? (KAnim.Build.Symbol) null : this.symbols[(int) index];

    public Texture2D GetTexture(int index)
    {
      if (index < 0 || index >= this.textureCount)
        Debug.LogError((object) ("Invalid texture index:" + (object) index));
      return KAnimBatchManager.Instance().GetBatchGroupData(this.batchTag).GetTexure(this.textureStartIdx + index);
    }

    public int GetSymbolOffset(KAnimHashedString symbol_name)
    {
      for (int index = 0; index < this.symbols.Length; ++index)
      {
        if (this.symbols[index].hash == symbol_name)
          return index;
      }
      return -1;
    }

    public KAnim.Build.Symbol GetSymbol(KAnimHashedString symbol_name)
    {
      for (int index = 0; index < this.symbols.Length; ++index)
      {
        if (this.symbols[index].hash == symbol_name)
          return this.symbols[index];
      }
      return (KAnim.Build.Symbol) null;
    }

    public override string ToString() => this.name;

    [Serializable]
    public class SymbolFrame : IComparable<KAnim.Build.SymbolFrame>
    {
      public int sourceFrameNum;
      public int duration;
      public KAnimHashedString fileNameHash;
      public Vector2 uvMin;
      public Vector2 uvMax;
      public Vector2 bboxMin;
      public Vector2 bboxMax;

      public int CompareTo(KAnim.Build.SymbolFrame obj) => this.sourceFrameNum.CompareTo(obj.sourceFrameNum);
    }

    public struct SymbolFrameInstance
    {
      public KAnim.Build.SymbolFrame symbolFrame;
      public int buildImageIdx;
      public int symbolIdx;
    }

    [DebuggerDisplay("{hash} {path} {folder} {colourChannel}")]
    [Serializable]
    public class Symbol : IComparable
    {
      [NonSerialized]
      public KAnim.Build build;
      public KAnimHashedString hash;
      public KAnimHashedString path;
      public KAnimHashedString folder;
      public KAnimHashedString colourChannel;
      public int flags;
      public int firstFrameIdx;
      public int numFrames;
      public int numLookupFrames;
      public int[] frameLookup;
      public int index;
      public int symbolIndexInSourceBuild;

      public int GetFrameIdx(int frame)
      {
        if (this.frameLookup == null)
          Debug.LogErrorFormat("Cant get frame [{2}] because Symbol [{0}] for build [{1}] batch [{3}] has no frameLookup", (object) this.hash.ToString(), (object) this.build.name, (object) frame, (object) this.build.batchTag.ToString());
        if (this.frameLookup.Length == 0 || frame >= this.frameLookup.Length)
          return -1;
        frame = Math.Min(frame, this.frameLookup.Length - 1);
        return this.frameLookup[frame];
      }

      public bool HasFrame(int frame) => this.GetFrameIdx(frame) >= 0;

      public KAnim.Build.SymbolFrameInstance GetFrame(int frame)
      {
        int frameIdx = this.GetFrameIdx(frame);
        return KAnimBatchManager.Instance().GetBatchGroupData(this.build.batchTag).GetSymbolFrameInstance(frameIdx);
      }

      public int CompareTo(object obj)
      {
        if (obj == null)
          return 1;
        return obj.GetType() == typeof (HashedString) ? this.hash.HashValue.CompareTo(((HashedString) obj).HashValue) : this.hash.HashValue.CompareTo(((KAnim.Build.Symbol) obj).hash.HashValue);
      }

      public bool HasFlag(KAnim.SymbolFlags flag) => (uint) ((KAnim.SymbolFlags) this.flags & flag) > 0U;

      public KAnim.Build.Symbol Copy()
      {
        KAnim.Build.Symbol symbol = new KAnim.Build.Symbol();
        symbol.hash = this.hash;
        symbol.path = this.path;
        symbol.folder = this.folder;
        symbol.colourChannel = this.colourChannel;
        symbol.flags = this.flags;
        symbol.firstFrameIdx = this.firstFrameIdx;
        symbol.numFrames = this.numFrames;
        symbol.numLookupFrames = this.numLookupFrames;
        symbol.frameLookup = new int[this.frameLookup.Length];
        symbol.symbolIndexInSourceBuild = this.symbolIndexInSourceBuild;
        Array.Copy((Array) this.frameLookup, (Array) symbol.frameLookup, symbol.frameLookup.Length);
        return symbol;
      }
    }
  }
}
