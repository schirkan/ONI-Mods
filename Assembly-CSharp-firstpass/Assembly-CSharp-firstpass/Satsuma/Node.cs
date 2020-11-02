// Decompiled with JetBrains decompiler
// Type: Satsuma.Node
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Satsuma
{
  public struct Node : IEquatable<Node>
  {
    public long Id { get; private set; }

    public Node(long id)
      : this()
      => this.Id = id;

    public static Node Invalid => new Node(0L);

    public bool Equals(Node other) => this.Id == other.Id;

    public override bool Equals(object obj) => obj is Node other && this.Equals(other);

    public override int GetHashCode() => this.Id.GetHashCode();

    public override string ToString() => "#" + (object) this.Id;

    public static bool operator ==(Node a, Node b) => a.Equals(b);

    public static bool operator !=(Node a, Node b) => !(a == b);
  }
}
