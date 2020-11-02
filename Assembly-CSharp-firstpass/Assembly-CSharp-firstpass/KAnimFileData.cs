// Decompiled with JetBrains decompiler
// Type: KAnimFileData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;

[DebuggerDisplay("{name}")]
public class KAnimFileData
{
  public const int NO_RECORD = -1;
  public int index;
  public HashedString batchTag;
  public int buildIndex;
  public HashedString animBatchTag;
  public int firstAnimIndex;
  public int animCount;
  public int frameCount;
  public int firstElementIndex;
  public int elementCount;
  public int maxVisSymbolFrames;

  public string name { get; private set; }

  public KAnimHashedString hashName { get; private set; }

  public KAnimFileData(string name)
  {
    this.name = name;
    this.firstAnimIndex = -1;
    this.buildIndex = -1;
    this.firstElementIndex = -1;
    this.animCount = 0;
    this.frameCount = 0;
    this.elementCount = 0;
    this.maxVisSymbolFrames = 0;
    this.hashName = new KAnimHashedString(name);
  }

  public KAnim.Build build
  {
    get
    {
      if (this.buildIndex == -1)
        return (KAnim.Build) null;
      KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.batchTag);
      if (batchGroupData == null)
        Debug.LogErrorFormat("[{0}] No such batch group [{1}]", (object) this.name, (object) this.batchTag.ToString());
      return batchGroupData.GetBuild(this.buildIndex);
    }
  }

  public KAnim.Anim GetAnim(int index)
  {
    Debug.Assert(index >= 0 && index < this.animCount);
    KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.animBatchTag);
    if (batchGroupData == null)
      Debug.LogError((object) string.Format("[{0}] No such batch group [{1}]", (object) this.name, (object) this.animBatchTag.ToString()));
    return batchGroupData.GetAnim(index + this.firstAnimIndex);
  }

  public KAnim.Anim.FrameElement GetAnimFrameElement(int index)
  {
    Debug.Assert(index >= 0 && index < this.elementCount);
    KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.animBatchTag);
    if (batchGroupData == null)
      Debug.LogErrorFormat("[{0}] No such batch group [{1}]", (object) this.name, (object) this.animBatchTag.ToString());
    return batchGroupData.GetFrameElement(this.firstElementIndex + index);
  }

  public KAnim.Anim.FrameElement FindAnimFrameElement(KAnimHashedString symbolName) => KAnimBatchManager.Instance().GetBatchGroupData(this.animBatchTag).frameElements.Find((Predicate<KAnim.Anim.FrameElement>) (match => match.symbol == symbolName));
}
