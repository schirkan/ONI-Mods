// Decompiled with JetBrains decompiler
// Type: KBatchGroupData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class KBatchGroupData
{
  public const int SIZE_OF_SYMBOL_FRAME_ELEMENT = 16;
  public const int SIZE_OF_ANIM_FRAME = 4;
  public const int SIZE_OF_ANIM_FRAME_ELEMENT = 16;
  private const int MAX_VISIBLE_SYMBOLS = 120;
  public const int MAX_GROUP_SIZE = 30;
  private const int NULL_DATA_FRAME_ID = -1010;

  public HashedString groupID { get; private set; }

  public bool isSwap { get; private set; }

  public int maxVisibleSymbols { get; private set; }

  public int maxSymbolsPerBuild => this.frameElementSymbols.Count;

  public int maxSymbolFrameInstancesPerbuild => this.symbolFrameInstances.Count;

  public int animDataStartOffset => this.symbolFrameInstances.Count * 16;

  public List<KAnim.Anim> anims { get; private set; }

  public Dictionary<KAnimHashedString, int> animIndex { get; private set; }

  public Dictionary<KAnimHashedString, int> animCount { get; private set; }

  public List<KAnim.Anim.Frame> animFrames { get; private set; }

  public List<KAnim.Anim.FrameElement> frameElements { get; private set; }

  public List<KAnim.Build> builds { get; private set; }

  public List<KAnim.Build.Symbol> frameElementSymbols { get; private set; }

  public Dictionary<KAnimHashedString, int> frameElementSymbolIndices { get; private set; }

  public List<KAnim.Build.SymbolFrameInstance> symbolFrameInstances { get; private set; }

  public Dictionary<KAnimHashedString, int> textureStartIndex { get; private set; }

  public Dictionary<KAnimHashedString, int> firstSymbolIndex { get; private set; }

  public List<Texture2D> textures { get; private set; }

  public KBatchGroupData(HashedString id)
  {
    this.groupID = id;
    this.maxVisibleSymbols = 1;
    this.Init();
  }

  private void Init()
  {
    this.anims = new List<KAnim.Anim>();
    this.animIndex = new Dictionary<KAnimHashedString, int>();
    this.animCount = new Dictionary<KAnimHashedString, int>();
    this.animFrames = new List<KAnim.Anim.Frame>();
    this.frameElements = new List<KAnim.Anim.FrameElement>();
    this.builds = new List<KAnim.Build>();
    this.frameElementSymbols = new List<KAnim.Build.Symbol>();
    this.frameElementSymbolIndices = new Dictionary<KAnimHashedString, int>();
    this.symbolFrameInstances = new List<KAnim.Build.SymbolFrameInstance>();
    this.textures = new List<Texture2D>();
    this.textureStartIndex = new Dictionary<KAnimHashedString, int>();
    this.firstSymbolIndex = new Dictionary<KAnimHashedString, int>();
  }

  public void FreeResources()
  {
    if (this.anims != null)
    {
      this.anims.Clear();
      this.anims = (List<KAnim.Anim>) null;
    }
    if (this.animIndex != null)
    {
      this.animIndex.Clear();
      this.animIndex = (Dictionary<KAnimHashedString, int>) null;
    }
    if (this.animCount != null)
    {
      this.animCount.Clear();
      this.animCount = (Dictionary<KAnimHashedString, int>) null;
    }
    if (this.animFrames != null)
    {
      this.animFrames.Clear();
      this.animFrames = (List<KAnim.Anim.Frame>) null;
    }
    if (this.frameElements != null)
    {
      this.frameElements.Clear();
      this.frameElements = (List<KAnim.Anim.FrameElement>) null;
    }
    if (this.builds != null)
    {
      this.builds.Clear();
      this.builds = (List<KAnim.Build>) null;
    }
    if (this.frameElementSymbols != null)
    {
      this.frameElementSymbols.Clear();
      this.frameElementSymbols = (List<KAnim.Build.Symbol>) null;
    }
    if (this.symbolFrameInstances != null)
    {
      this.symbolFrameInstances.Clear();
      this.symbolFrameInstances = (List<KAnim.Build.SymbolFrameInstance>) null;
    }
    if (this.textures != null)
    {
      this.textures.Clear();
      this.textures = (List<Texture2D>) null;
    }
    if (this.textureStartIndex != null)
    {
      this.textureStartIndex.Clear();
      this.textureStartIndex = (Dictionary<KAnimHashedString, int>) null;
    }
    if (this.firstSymbolIndex == null)
      return;
    this.firstSymbolIndex.Clear();
    this.firstSymbolIndex = (Dictionary<KAnimHashedString, int>) null;
  }

  public KAnim.Build AddNewBuildFile(KAnimHashedString fileHash)
  {
    this.textureStartIndex.Add(fileHash, this.textures.Count);
    this.firstSymbolIndex.Add(fileHash, this.GetSymbolCount());
    KAnim.Build build = new KAnim.Build();
    build.textureStartIdx = this.textures.Count;
    build.fileHash = fileHash;
    build.index = this.builds.Count;
    this.builds.Add(build);
    return build;
  }

  public void AddTextures(List<Texture2D> buildtextures) => this.textures.AddRange((IEnumerable<Texture2D>) buildtextures);

  public void AddAnim(KAnim.Anim anim)
  {
    Debug.Assert(anim.index == this.anims.Count);
    this.anims.Add(anim);
  }

  public KAnim.Anim GetAnim(int anim)
  {
    if (anim < 0 || anim >= this.anims.Count)
      Debug.LogError((object) string.Format("Anim [{0}] out of range [{1}] in batch [{2}]", (object) anim, (object) this.anims.Count, (object) this.groupID));
    return this.anims[anim];
  }

  public KAnim.Build GetBuild(int index) => this.builds[index];

  public void UpdateMaxVisibleSymbols(int newCount) => this.maxVisibleSymbols = Mathf.Min(120, Mathf.Max(this.maxVisibleSymbols, newCount));

  public KAnim.Build.Symbol GetSymbol(KAnimHashedString symbol_name)
  {
    foreach (KAnim.Build.Symbol frameElementSymbol in this.frameElementSymbols)
    {
      if (frameElementSymbol.hash == symbol_name)
        return frameElementSymbol;
    }
    return (KAnim.Build.Symbol) null;
  }

  public KAnim.Build.Symbol GetSymbol(int index) => index >= 0 && index < this.frameElementSymbols.Count ? this.frameElementSymbols[index] : (KAnim.Build.Symbol) null;

  public void AddBuildSymbol(KAnim.Build.Symbol symbol)
  {
    if (!this.frameElementSymbolIndices.ContainsKey(symbol.hash))
      this.frameElementSymbolIndices.Add(symbol.hash, this.frameElementSymbols.Count);
    this.frameElementSymbols.Add(symbol);
  }

  public int GetSymbolCount() => this.frameElementSymbols.Count;

  public KAnim.Build.SymbolFrameInstance GetSymbolFrameInstance(int index)
  {
    if (index >= 0 && index < this.symbolFrameInstances.Count)
      return this.symbolFrameInstances[index];
    return new KAnim.Build.SymbolFrameInstance()
    {
      symbolIdx = -1
    };
  }

  public Texture2D GetTexure(int index) => index < 0 || this.textures == null || index >= this.textures.Count ? (Texture2D) null : this.textures[index];

  public KAnim.Build.Symbol GetBuildSymbol(int idx) => this.frameElementSymbols == null || idx < 0 || idx >= this.frameElementSymbols.Count ? (KAnim.Build.Symbol) null : this.frameElementSymbols[idx];

  public KAnim.Anim.Frame GetFrame(int index) => index < 0 || index >= this.animFrames.Count ? KAnim.Anim.Frame.InvalidFrame : this.animFrames[index];

  public KAnim.Anim.FrameElement GetFrameElement(int index) => this.frameElements[index];

  public List<KAnim.Anim.Frame> GetAnimFrames() => this.animFrames;

  public List<KAnim.Anim.FrameElement> GetAnimFrameElements() => this.frameElements;

  public int GetBuildSymbolFrameCount() => this.symbolFrameInstances.Count;

  public void WriteAnimData(int start_index, float[] data)
  {
    List<KAnim.Anim.Frame> animFrames = this.GetAnimFrames();
    List<KAnim.Anim.FrameElement> animFrameElements = this.GetAnimFrameElements();
    int num = 1 + (animFrames.Count == 0 ? this.symbolFrameInstances.Count : animFrames.Count);
    if (animFrames.Count == 0 && this.symbolFrameInstances.Count == 0 && animFrameElements.Count == 0)
      Debug.LogError((object) ("Eh, no data " + (object) animFrames.Count + " " + (object) this.symbolFrameInstances.Count + " " + (object) animFrameElements.Count));
    data[start_index++] = (float) num;
    data[start_index++] = (float) animFrames.Count;
    data[start_index++] = (float) animFrameElements.Count;
    data[start_index++] = (float) this.symbolFrameInstances.Count;
    if (animFrames.Count == 0)
    {
      for (int index = 0; index < this.symbolFrameInstances.Count; ++index)
      {
        this.WriteAnimFrame(data, start_index, index, index, 1, index);
        start_index += 4;
      }
      for (int index = 0; index < this.symbolFrameInstances.Count; ++index)
      {
        this.WriteAnimFrameElement(data, start_index, index, index, Matrix2x3.identity, Color.white, 0);
        start_index += 16;
      }
    }
    else
    {
      for (int index = 0; index < animFrames.Count; ++index)
      {
        this.Write(data, start_index, index, animFrames[index]);
        start_index += 4;
      }
      for (int index = 0; index < animFrameElements.Count; ++index)
      {
        KAnim.Anim.FrameElement element = animFrameElements[index];
        if (element.symbol == KGlobalAnimParser.MISSING_SYMBOL)
        {
          this.WriteAnimFrameElement(data, start_index, -1, index, Matrix2x3.identity, Color.white, 0);
        }
        else
        {
          KAnim.Build.Symbol buildSymbol = this.GetBuildSymbol(element.symbolIdx);
          if (buildSymbol == null)
            Debug.LogError((object) ("Missing symbol for Anim Frame Element: [" + HashCache.Get().Get(element.symbol) + ": " + (object) element.symbol + "]"));
          int frameIdx = buildSymbol.GetFrameIdx(element.frame);
          this.Write(data, start_index, frameIdx, index, element);
        }
        start_index += 16;
      }
    }
  }

  public int GetFirstIndex(KAnimHashedString symbol) => this.frameElementSymbols.FindIndex((Predicate<KAnim.Build.Symbol>) (fes => fes.hash == symbol));

  public int GetSymbolIndex(KAnimHashedString symbol)
  {
    int num = 0;
    return !this.frameElementSymbolIndices.TryGetValue(symbol, out num) ? -1 : num;
  }

  public int WriteBuildData(
    List<KAnim.Build.SymbolFrameInstance> symbol_frame_instances,
    float[] data)
  {
    int num;
    for (num = 0; num < symbol_frame_instances.Count; ++num)
      this.Write(data, num * 16, num, this.symbolFrameInstances[num].buildImageIdx, symbol_frame_instances[num]);
    return num * 16;
  }

  private void Write(
    float[] data,
    int startIndex,
    int thisFrameIndex,
    int atlasIndex,
    KAnim.Build.SymbolFrameInstance symbol_frame_instance)
  {
    data[startIndex++] = (float) atlasIndex;
    data[startIndex++] = (float) thisFrameIndex;
    data[startIndex++] = (float) symbol_frame_instance.symbolIdx;
    KAnim.Build.SymbolFrame symbolFrame = symbol_frame_instance.symbolFrame;
    KAnim.Build.Symbol buildSymbol = this.GetBuildSymbol(symbol_frame_instance.symbolIdx);
    if (buildSymbol == null || symbolFrame == null)
    {
      data[startIndex++] = 0.0f;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 0.0f;
    }
    else
    {
      data[startIndex++] = (float) buildSymbol.numFrames;
      data[startIndex++] = (float) buildSymbol.flags;
      data[startIndex++] = !this.firstSymbolIndex.ContainsKey(buildSymbol.build.fileHash) ? 0.0f : (float) this.firstSymbolIndex[buildSymbol.build.fileHash];
      data[startIndex++] = (float) buildSymbol.symbolIndexInSourceBuild;
    }
    data[startIndex++] = 3.452817E+09f;
    if (symbolFrame == null)
      return;
    data[startIndex++] = symbolFrame.bboxMin.x;
    data[startIndex++] = symbolFrame.bboxMin.y;
    data[startIndex++] = symbolFrame.bboxMax.x;
    data[startIndex++] = symbolFrame.bboxMax.y;
    data[startIndex++] = symbolFrame.uvMin.x;
    data[startIndex++] = symbolFrame.uvMin.y;
    data[startIndex++] = symbolFrame.uvMax.x;
    data[startIndex++] = symbolFrame.uvMax.y;
  }

  private void WriteAnimFrame(
    float[] data,
    int startIndex,
    int firstElementIdx,
    int idx,
    int numElements,
    int thisFrameIndex)
  {
    data[startIndex++] = (float) firstElementIdx;
    data[startIndex++] = (float) numElements;
    data[startIndex++] = (float) thisFrameIndex;
    data[startIndex++] = (float) idx;
  }

  private void Write(float[] data, int startIndex, int thisFrameIndex, KAnim.Anim.Frame frame) => this.WriteAnimFrame(data, startIndex, frame.firstElementIdx, frame.idx, frame.numElements, thisFrameIndex);

  private void WriteAnimFrameElement(
    float[] data,
    int startIndex,
    int symbolFrameIdx,
    int thisFrameIndex,
    Matrix2x3 transform,
    Color colour,
    int flags)
  {
    if (symbolFrameIdx != -1010)
    {
      data[startIndex++] = (float) symbolFrameIdx;
      data[startIndex++] = (float) thisFrameIndex;
      data[startIndex++] = (float) flags;
      data[startIndex++] = 0.0f;
      data[startIndex++] = colour.r;
      data[startIndex++] = colour.g;
      data[startIndex++] = colour.b;
      data[startIndex++] = colour.a;
      data[startIndex++] = transform.m00;
      data[startIndex++] = transform.m01;
      data[startIndex++] = transform.m02;
      data[startIndex++] = 2.880155E+09f;
      data[startIndex++] = transform.m10;
      data[startIndex++] = transform.m11;
      data[startIndex++] = transform.m12;
      data[startIndex++] = 3.166486E+09f;
    }
    else
    {
      data[startIndex++] = (float) symbolFrameIdx;
      data[startIndex++] = (float) thisFrameIndex;
      data[startIndex++] = (float) flags;
      data[startIndex++] = -1f;
      data[startIndex++] = colour.r;
      data[startIndex++] = colour.g;
      data[startIndex++] = colour.b;
      data[startIndex++] = colour.a;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 2.880155E+09f;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 0.0f;
      data[startIndex++] = 3.166486E+09f;
    }
  }

  private void WriteNullFrameElement(float[] data, int startIndex, int thisFrameIndex) => this.WriteAnimFrameElement(data, startIndex, -1010, thisFrameIndex, Matrix2x3.identity, Color.black, 0);

  private void Write(
    float[] data,
    int startIndex,
    int symbolFrameIdx,
    int thisFrameIndex,
    KAnim.Anim.FrameElement element)
  {
    this.WriteAnimFrameElement(data, startIndex, symbolFrameIdx, thisFrameIndex, element.transform, element.multColour, element.flags);
  }
}
