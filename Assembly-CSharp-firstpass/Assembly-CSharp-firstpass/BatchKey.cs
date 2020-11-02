// Decompiled with JetBrains decompiler
// Type: BatchKey
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

public struct BatchKey : IEquatable<BatchKey>
{
  private float _z;
  private int _layer;
  private KAnimBatchGroup.MaterialType _materialType;
  private HashedString _groupID;
  private Vector2I _idx;
  private int _hash;

  private BatchKey(KAnimConverter.IAnimConverter controller)
  {
    this._layer = controller.GetLayer();
    this._groupID = controller.GetBatchGroupID();
    this._materialType = controller.GetMaterialType();
    this._z = controller.GetZ();
    this._idx = KAnimBatchManager.ControllerToChunkXY(controller);
    this._hash = 0;
  }

  private BatchKey(KAnimConverter.IAnimConverter controller, Vector2I idx)
    : this(controller)
    => this._idx = idx;

  private void CalculateHash() => this._hash = (int) ((KAnimBatchGroup.MaterialType) (this._z.GetHashCode() ^ this._layer) ^ this._materialType ^ (KAnimBatchGroup.MaterialType) this._groupID.HashValue ^ (KAnimBatchGroup.MaterialType) this._idx.GetHashCode());

  public static BatchKey Create(KAnimConverter.IAnimConverter controller, Vector2I idx)
  {
    BatchKey batchKey = new BatchKey(controller, idx);
    batchKey.CalculateHash();
    return batchKey;
  }

  public static BatchKey Create(KAnimConverter.IAnimConverter controller)
  {
    BatchKey batchKey = new BatchKey(controller);
    batchKey.CalculateHash();
    return batchKey;
  }

  public bool Equals(BatchKey other) => (double) this._z == (double) other._z && this._layer == other._layer && (this._materialType == other._materialType && this._groupID == other._groupID) && this._idx == other._idx;

  public override int GetHashCode() => this._hash;

  public float z => this._z;

  public int layer => this._layer;

  public HashedString groupID => this._groupID;

  public Vector2I idx => this._idx;

  public KAnimBatchGroup.MaterialType materialType => this._materialType;

  public int hash => this._hash;

  public override string ToString() => "[" + (object) this.idx.x + "," + (object) this.idx.y + "] [" + (object) this.groupID.HashValue + "] [" + (object) this.layer + "] [" + (object) this.z + "]" + this.materialType.ToString();
}
