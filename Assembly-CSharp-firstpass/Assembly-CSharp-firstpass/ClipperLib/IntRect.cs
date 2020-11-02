// Decompiled with JetBrains decompiler
// Type: ClipperLib.IntRect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ClipperLib
{
  public struct IntRect
  {
    public long left;
    public long top;
    public long right;
    public long bottom;

    public IntRect(long l, long t, long r, long b)
    {
      this.left = l;
      this.top = t;
      this.right = r;
      this.bottom = b;
    }

    public IntRect(IntRect ir)
    {
      this.left = ir.left;
      this.top = ir.top;
      this.right = ir.right;
      this.bottom = ir.bottom;
    }
  }
}
