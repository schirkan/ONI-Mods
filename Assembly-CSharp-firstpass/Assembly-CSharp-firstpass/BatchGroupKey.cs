// Decompiled with JetBrains decompiler
// Type: BatchGroupKey
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

public struct BatchGroupKey : IEquatable<BatchGroupKey>
{
  private HashedString _groupID;

  public BatchGroupKey(HashedString group_id) => this._groupID = group_id;

  public bool Equals(BatchGroupKey other) => this._groupID == other._groupID;

  public override int GetHashCode() => this._groupID.HashValue;

  public HashedString groupID => this._groupID;
}
