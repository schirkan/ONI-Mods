// Decompiled with JetBrains decompiler
// Type: MIConvexHull.ConnectorList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace MIConvexHull
{
  internal sealed class ConnectorList
  {
    private FaceConnector last;

    public FaceConnector First { get; private set; }

    private void AddFirst(FaceConnector connector)
    {
      this.First.Previous = connector;
      connector.Next = this.First;
      this.First = connector;
    }

    public void Add(FaceConnector element)
    {
      if (this.last != null)
        this.last.Next = element;
      element.Previous = this.last;
      this.last = element;
      if (this.First != null)
        return;
      this.First = element;
    }

    public void Remove(FaceConnector connector)
    {
      if (connector.Previous != null)
        connector.Previous.Next = connector.Next;
      else if (connector.Previous == null)
        this.First = connector.Next;
      if (connector.Next != null)
        connector.Next.Previous = connector.Previous;
      else if (connector.Next == null)
        this.last = connector.Previous;
      connector.Next = (FaceConnector) null;
      connector.Previous = (FaceConnector) null;
    }
  }
}
