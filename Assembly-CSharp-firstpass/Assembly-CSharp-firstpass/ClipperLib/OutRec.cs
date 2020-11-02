// Decompiled with JetBrains decompiler
// Type: ClipperLib.OutRec
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ClipperLib
{
  internal class OutRec
  {
    internal int Idx;
    internal bool IsHole;
    internal bool IsOpen;
    internal OutRec FirstLeft;
    internal OutPt Pts;
    internal OutPt BottomPt;
    internal PolyNode PolyNode;
  }
}
