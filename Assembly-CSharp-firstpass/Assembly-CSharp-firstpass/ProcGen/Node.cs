// Decompiled with JetBrains decompiler
// Type: ProcGen.Node
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using UnityEngine;

namespace ProcGen
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Node
  {
    [Serialize]
    public TagSet tags = new TagSet();
    public TagSet featureSpecificTags = new TagSet();
    public TagSet biomeSpecificTags = new TagSet();

    public Satsuma.Node node { get; private set; }

    [Serialize]
    public string type { get; private set; }

    public void SetType(string newtype) => this.type = newtype;

    [Serialize]
    public Vector2 position { get; private set; }

    public void SetPosition(Vector2 newPos) => this.position = newPos;

    public Node()
    {
    }

    public Node(string type) => this.type = type;

    public Node(Node other)
    {
      this.position = other.position;
      this.node = other.node;
      this.type = other.type;
      this.tags = new TagSet(other.tags);
      this.featureSpecificTags = new TagSet(other.featureSpecificTags);
      this.biomeSpecificTags = new TagSet(other.biomeSpecificTags);
    }

    public Node(Satsuma.Node node, string type)
    {
      this.node = node;
      this.type = type;
    }
  }
}
