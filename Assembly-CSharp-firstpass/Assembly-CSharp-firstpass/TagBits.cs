// Decompiled with JetBrains decompiler
// Type: TagBits
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public struct TagBits
{
  private static Dictionary<Tag, int> tagTable = new Dictionary<Tag, int>();
  private static List<Tag> inverseTagTable = new List<Tag>();
  private const int Capacity = 384;
  private ulong bits0;
  private ulong bits1;
  private ulong bits2;
  private ulong bits3;
  private ulong bits4;
  private ulong bits5;
  public static TagBits None = new TagBits();

  public TagBits(ref TagBits other)
  {
    this.bits0 = other.bits0;
    this.bits1 = other.bits1;
    this.bits2 = other.bits2;
    this.bits3 = other.bits3;
    this.bits4 = other.bits4;
    this.bits5 = other.bits5;
  }

  public TagBits(Tag tag)
  {
    this.bits0 = 0UL;
    this.bits1 = 0UL;
    this.bits2 = 0UL;
    this.bits3 = 0UL;
    this.bits4 = 0UL;
    this.bits5 = 0UL;
    this.SetTag(tag);
  }

  public TagBits(Tag[] tags)
  {
    this.bits0 = 0UL;
    this.bits1 = 0UL;
    this.bits2 = 0UL;
    this.bits3 = 0UL;
    this.bits4 = 0UL;
    this.bits5 = 0UL;
    if (tags == null)
      return;
    foreach (Tag tag in tags)
      this.SetTag(tag);
  }

  public List<Tag> GetTagsVerySlow()
  {
    List<Tag> tags = new List<Tag>();
    this.GetTagsVerySlow(0, this.bits0, tags);
    this.GetTagsVerySlow(1, this.bits1, tags);
    this.GetTagsVerySlow(2, this.bits2, tags);
    this.GetTagsVerySlow(3, this.bits3, tags);
    this.GetTagsVerySlow(4, this.bits4, tags);
    this.GetTagsVerySlow(5, this.bits5, tags);
    return tags;
  }

  private void GetTagsVerySlow(int bits_idx, ulong bits, List<Tag> tags)
  {
    for (int index1 = 0; index1 < 64; ++index1)
    {
      if (((long) bits & 1L << index1) != 0L)
      {
        int index2 = 64 * bits_idx + index1;
        tags.Add(TagBits.inverseTagTable[index2]);
      }
    }
  }

  private static int ManifestFlagIndex(Tag tag)
  {
    int num1;
    if (TagBits.tagTable.TryGetValue(tag, out num1))
      return num1;
    int count = TagBits.tagTable.Count;
    TagBits.tagTable.Add(tag, count);
    TagBits.inverseTagTable.Add(tag);
    DebugUtil.Assert(TagBits.inverseTagTable.Count == count + 1);
    if (TagBits.tagTable.Count >= 384)
    {
      string str = "Out of tag bits:\n";
      int num2 = 0;
      foreach (KeyValuePair<Tag, int> keyValuePair in TagBits.tagTable)
      {
        str = str + keyValuePair.Key.ToString() + ", ";
        ++num2;
        if (num2 % 64 == 0)
          str += "\n";
      }
      Debug.LogError((object) str);
    }
    return count;
  }

  public void SetTag(Tag tag)
  {
    int num = TagBits.ManifestFlagIndex(tag);
    if (num < 64)
      this.bits0 |= (ulong) (1L << num);
    else if (num < 128)
      this.bits1 |= (ulong) (1L << num);
    else if (num < 192)
      this.bits2 |= (ulong) (1L << num);
    else if (num < 256)
      this.bits3 |= (ulong) (1L << num);
    else if (num < 320)
      this.bits4 |= (ulong) (1L << num);
    else if (num < 384)
      this.bits5 |= (ulong) (1L << num);
    else
      Debug.LogError((object) "Out of bits!");
  }

  public void Clear(Tag tag)
  {
    int num = TagBits.ManifestFlagIndex(tag);
    if (num < 64)
      this.bits0 &= (ulong) ~(1L << num);
    else if (num < 128)
      this.bits1 &= (ulong) ~(1L << num);
    else if (num < 192)
      this.bits2 &= (ulong) ~(1L << num);
    else if (num < 256)
      this.bits3 &= (ulong) ~(1L << num);
    else if (num < 320)
      this.bits4 &= (ulong) ~(1L << num);
    else if (num < 384)
      this.bits5 &= (ulong) ~(1L << num);
    else
      Debug.LogError((object) "Out of bits!");
  }

  public void ClearAll()
  {
    this.bits0 = 0UL;
    this.bits1 = 0UL;
    this.bits2 = 0UL;
    this.bits3 = 0UL;
    this.bits4 = 0UL;
    this.bits5 = 0UL;
  }

  public bool HasAll(ref TagBits tag_bits) => ((long) this.bits0 & (long) tag_bits.bits0) == (long) tag_bits.bits0 && ((long) this.bits1 & (long) tag_bits.bits1) == (long) tag_bits.bits1 && (((long) this.bits2 & (long) tag_bits.bits2) == (long) tag_bits.bits2 && ((long) this.bits3 & (long) tag_bits.bits3) == (long) tag_bits.bits3) && ((long) this.bits4 & (long) tag_bits.bits4) == (long) tag_bits.bits4 && ((long) this.bits5 & (long) tag_bits.bits5) == (long) tag_bits.bits5;

  public bool HasAny(ref TagBits tag_bits) => (ulong) ((long) this.bits0 & (long) tag_bits.bits0 | (long) this.bits1 & (long) tag_bits.bits1 | (long) this.bits2 & (long) tag_bits.bits2 | (long) this.bits3 & (long) tag_bits.bits3 | (long) this.bits4 & (long) tag_bits.bits4 | (long) this.bits5 & (long) tag_bits.bits5) > 0UL;

  public bool AreEqual(ref TagBits tag_bits) => (long) tag_bits.bits0 == (long) this.bits0 && (long) tag_bits.bits1 == (long) this.bits1 && ((long) tag_bits.bits2 == (long) this.bits2 && (long) tag_bits.bits3 == (long) this.bits3) && (long) tag_bits.bits4 == (long) this.bits4 && (long) tag_bits.bits5 == (long) this.bits5;

  public void And(ref TagBits rhs)
  {
    this.bits0 &= rhs.bits0;
    this.bits1 &= rhs.bits1;
    this.bits2 &= rhs.bits2;
    this.bits3 &= rhs.bits3;
    this.bits4 &= rhs.bits4;
    this.bits5 &= rhs.bits5;
  }

  public void Or(ref TagBits rhs)
  {
    this.bits0 |= rhs.bits0;
    this.bits1 |= rhs.bits1;
    this.bits2 |= rhs.bits2;
    this.bits3 |= rhs.bits3;
    this.bits4 |= rhs.bits4;
    this.bits5 |= rhs.bits5;
  }

  public void Xor(ref TagBits rhs)
  {
    this.bits0 ^= rhs.bits0;
    this.bits1 ^= rhs.bits1;
    this.bits2 ^= rhs.bits2;
    this.bits3 ^= rhs.bits3;
    this.bits4 ^= rhs.bits4;
    this.bits5 ^= rhs.bits5;
  }

  public void Complement()
  {
    this.bits0 = ~this.bits0;
    this.bits1 = ~this.bits1;
    this.bits2 = ~this.bits2;
    this.bits3 = ~this.bits3;
    this.bits4 = ~this.bits4;
    this.bits5 = ~this.bits5;
  }

  public static TagBits MakeComplement(ref TagBits rhs)
  {
    TagBits tagBits = new TagBits(ref rhs);
    tagBits.Complement();
    return tagBits;
  }
}
