// Decompiled with JetBrains decompiler
// Type: Expression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

[DebuggerDisplay("{face.hash} {priority}")]
public class Expression : Resource
{
  public Face face;
  public int priority;

  public Expression(string id, ResourceSet parent, Face face)
    : base(id, parent)
    => this.face = face;
}
