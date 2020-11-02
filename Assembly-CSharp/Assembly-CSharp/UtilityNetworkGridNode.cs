// Decompiled with JetBrains decompiler
// Type: UtilityNetworkGridNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
  public UtilityConnections connections;
  public int networkIdx;
  public const int InvalidNetworkIdx = -1;

  public bool Equals(UtilityNetworkGridNode other) => this.connections == other.connections && this.networkIdx == other.networkIdx;

  public override bool Equals(object obj) => ((UtilityNetworkGridNode) obj).Equals(this);

  public override int GetHashCode() => base.GetHashCode();

  public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y) => x.Equals(y);

  public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y) => !x.Equals(y);
}
