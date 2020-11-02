// Decompiled with JetBrains decompiler
// Type: KCollider2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick
{
  [SerializeField]
  public Vector2 _offset;
  private Extents cachedExtents;
  private HandleVector<int>.Handle partitionerEntry;

  public Vector2 offset
  {
    get => this._offset;
    set
    {
      this._offset = value;
      this.MarkDirty();
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.autoRegisterSimRender = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(this.transform, new System.Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
    this.MarkDirty(true);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(this.transform, new System.Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }

  public void MarkDirty(bool force = false)
  {
    bool flag = force || this.partitionerEntry.IsValid();
    if (!flag)
      return;
    Extents extents = this.GetExtents();
    if (!force && this.cachedExtents.x == extents.x && (this.cachedExtents.y == extents.y && this.cachedExtents.width == extents.width) && this.cachedExtents.height == extents.height)
      return;
    this.cachedExtents = extents;
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    if (!flag)
      return;
    this.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, (object) this, this.cachedExtents, GameScenePartitioner.Instance.collisionLayer, (System.Action<object>) null);
  }

  private void OnMovementStateChanged(bool is_moving)
  {
    if (is_moving)
    {
      this.MarkDirty();
      SimAndRenderScheduler.instance.Add((object) this);
    }
    else
      SimAndRenderScheduler.instance.Remove((object) this);
  }

  private static void OnMovementStateChanged(Transform transform, bool is_moving) => transform.GetComponent<KCollider2D>().OnMovementStateChanged(is_moving);

  public void RenderEveryTick(float dt) => this.MarkDirty();

  public abstract bool Intersects(Vector2 pos);

  public abstract Extents GetExtents();

  public abstract Bounds bounds { get; }
}
