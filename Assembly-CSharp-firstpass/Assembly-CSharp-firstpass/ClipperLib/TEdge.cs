// Decompiled with JetBrains decompiler
// Type: ClipperLib.TEdge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ClipperLib
{
  internal class TEdge
  {
    internal IntPoint Bot;
    internal IntPoint Curr;
    internal IntPoint Top;
    internal IntPoint Delta;
    internal double Dx;
    internal PolyType PolyTyp;
    internal EdgeSide Side;
    internal int WindDelta;
    internal int WindCnt;
    internal int WindCnt2;
    internal int OutIdx;
    internal TEdge Next;
    internal TEdge Prev;
    internal TEdge NextInLML;
    internal TEdge NextInAEL;
    internal TEdge PrevInAEL;
    internal TEdge NextInSEL;
    internal TEdge PrevInSEL;
  }
}
