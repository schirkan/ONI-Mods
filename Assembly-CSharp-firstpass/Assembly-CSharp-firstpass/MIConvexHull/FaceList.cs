// Decompiled with JetBrains decompiler
// Type: MIConvexHull.FaceList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace MIConvexHull
{
  internal sealed class FaceList
  {
    private ConvexFaceInternal last;

    public ConvexFaceInternal First { get; private set; }

    private void AddFirst(ConvexFaceInternal face)
    {
      face.InList = true;
      this.First.Previous = face;
      face.Next = this.First;
      this.First = face;
    }

    public void Add(ConvexFaceInternal face)
    {
      if (face.InList)
      {
        if (this.First.VerticesBeyond.Count >= face.VerticesBeyond.Count)
          return;
        this.Remove(face);
        this.AddFirst(face);
      }
      else
      {
        face.InList = true;
        if (this.First != null && this.First.VerticesBeyond.Count < face.VerticesBeyond.Count)
        {
          this.First.Previous = face;
          face.Next = this.First;
          this.First = face;
        }
        else
        {
          if (this.last != null)
            this.last.Next = face;
          face.Previous = this.last;
          this.last = face;
          if (this.First != null)
            return;
          this.First = face;
        }
      }
    }

    public void Remove(ConvexFaceInternal face)
    {
      if (!face.InList)
        return;
      face.InList = false;
      if (face.Previous != null)
        face.Previous.Next = face.Next;
      else if (face.Previous == null)
        this.First = face.Next;
      if (face.Next != null)
        face.Next.Previous = face.Previous;
      else if (face.Next == null)
        this.last = face.Previous;
      face.Next = (ConvexFaceInternal) null;
      face.Previous = (ConvexFaceInternal) null;
    }
  }
}
