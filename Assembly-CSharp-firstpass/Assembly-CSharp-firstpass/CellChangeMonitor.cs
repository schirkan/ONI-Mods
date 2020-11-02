// Decompiled with JetBrains decompiler
// Type: CellChangeMonitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class CellChangeMonitor : Singleton<CellChangeMonitor>
{
  private Dictionary<int, CellChangeMonitor.CellChangedEntry> cellChangedHandlers = new Dictionary<int, CellChangeMonitor.CellChangedEntry>();
  private Dictionary<int, CellChangeMonitor.MovementStateChangedEntry> movementStateChangedHandlers = new Dictionary<int, CellChangeMonitor.MovementStateChangedEntry>();
  private HashSet<int> pendingDirtyTransforms = new HashSet<int>();
  private HashSet<int> dirtyTransforms = new HashSet<int>();
  private HashSet<int> movingTransforms = new HashSet<int>();
  private HashSet<int> previouslyMovingTransforms = new HashSet<int>();
  private Dictionary<int, int> transformLastKnownCell = new Dictionary<int, int>();
  private List<CellChangeMonitor.CellChangedEntry.Handler> cellChangedCallbacksToRun = new List<CellChangeMonitor.CellChangedEntry.Handler>();
  private List<System.Action<Transform, bool>> moveChangedCallbacksToRun = new List<System.Action<Transform, bool>>();
  private int gridWidth;

  public void MarkDirty(Transform transform)
  {
    if (this.gridWidth == 0)
      return;
    this.pendingDirtyTransforms.Add(transform.GetInstanceID());
    int childCount = transform.childCount;
    for (int index = 0; index < childCount; ++index)
      this.MarkDirty(transform.GetChild(index));
  }

  public bool IsMoving(Transform transform) => this.movingTransforms.Contains(transform.GetInstanceID());

  public void RegisterMovementStateChanged(Transform transform, System.Action<Transform, bool> handler)
  {
    int instanceId = transform.GetInstanceID();
    CellChangeMonitor.MovementStateChangedEntry stateChangedEntry = new CellChangeMonitor.MovementStateChangedEntry();
    if (!this.movementStateChangedHandlers.TryGetValue(instanceId, out stateChangedEntry))
    {
      stateChangedEntry = new CellChangeMonitor.MovementStateChangedEntry();
      stateChangedEntry.handlers = new List<System.Action<Transform, bool>>();
      stateChangedEntry.transform = transform;
    }
    stateChangedEntry.handlers.Add(handler);
    this.movementStateChangedHandlers[instanceId] = stateChangedEntry;
  }

  public void UnregisterMovementStateChanged(int instance_id, System.Action<Transform, bool> callback)
  {
    CellChangeMonitor.MovementStateChangedEntry stateChangedEntry = new CellChangeMonitor.MovementStateChangedEntry();
    if (!this.movementStateChangedHandlers.TryGetValue(instance_id, out stateChangedEntry))
      return;
    stateChangedEntry.handlers.Remove(callback);
    if (stateChangedEntry.handlers.Count != 0)
      return;
    this.movementStateChangedHandlers.Remove(instance_id);
  }

  public void UnregisterMovementStateChanged(Transform transform, System.Action<Transform, bool> callback) => this.UnregisterMovementStateChanged(transform.GetInstanceID(), callback);

  public int RegisterCellChangedHandler(Transform transform, System.Action callback, string debug_name)
  {
    int instanceId = transform.GetInstanceID();
    CellChangeMonitor.CellChangedEntry cellChangedEntry = new CellChangeMonitor.CellChangedEntry();
    if (!this.cellChangedHandlers.TryGetValue(instanceId, out cellChangedEntry))
    {
      cellChangedEntry = new CellChangeMonitor.CellChangedEntry();
      cellChangedEntry.transform = transform;
      cellChangedEntry.handlers = new List<CellChangeMonitor.CellChangedEntry.Handler>();
    }
    CellChangeMonitor.CellChangedEntry.Handler handler = new CellChangeMonitor.CellChangedEntry.Handler()
    {
      name = debug_name,
      callback = callback
    };
    cellChangedEntry.handlers.Add(handler);
    this.cellChangedHandlers[instanceId] = cellChangedEntry;
    return instanceId;
  }

  public void UnregisterCellChangedHandler(int instance_id, System.Action callback)
  {
    CellChangeMonitor.CellChangedEntry cellChangedEntry = new CellChangeMonitor.CellChangedEntry();
    if (!this.cellChangedHandlers.TryGetValue(instance_id, out cellChangedEntry))
      return;
    for (int index = 0; index < cellChangedEntry.handlers.Count; ++index)
    {
      if (!(cellChangedEntry.handlers[index].callback != callback))
      {
        cellChangedEntry.handlers.RemoveAt(index);
        break;
      }
    }
    if (cellChangedEntry.handlers.Count != 0)
      return;
    this.cellChangedHandlers.Remove(instance_id);
  }

  public void UnregisterCellChangedHandler(Transform transform, System.Action callback) => this.UnregisterCellChangedHandler(transform.GetInstanceID(), callback);

  public int PosToCell(Vector3 pos)
  {
    float x = pos.x;
    int num1 = (int) ((double) pos.y + 0.0500000007450581);
    int num2 = (int) x;
    int gridWidth = this.gridWidth;
    return num1 * gridWidth + num2;
  }

  public void SetGridSize(int grid_width, int grid_height) => this.gridWidth = grid_width;

  public void RenderEveryTick()
  {
    HashSet<int> pendingDirtyTransforms = this.pendingDirtyTransforms;
    this.pendingDirtyTransforms = this.dirtyTransforms;
    this.dirtyTransforms = pendingDirtyTransforms;
    this.pendingDirtyTransforms.Clear();
    this.previouslyMovingTransforms.Clear();
    HashSet<int> movingTransforms = this.previouslyMovingTransforms;
    this.previouslyMovingTransforms = this.movingTransforms;
    this.movingTransforms = movingTransforms;
    foreach (int dirtyTransform in this.dirtyTransforms)
    {
      CellChangeMonitor.CellChangedEntry cellChangedEntry = new CellChangeMonitor.CellChangedEntry();
      if (this.cellChangedHandlers.TryGetValue(dirtyTransform, out cellChangedEntry))
      {
        if (!((UnityEngine.Object) cellChangedEntry.transform == (UnityEngine.Object) null))
        {
          int num = -1;
          this.transformLastKnownCell.TryGetValue(dirtyTransform, out num);
          int cell = this.PosToCell(cellChangedEntry.transform.GetPosition());
          if (num != cell)
          {
            this.cellChangedCallbacksToRun.Clear();
            this.cellChangedCallbacksToRun.AddRange((IEnumerable<CellChangeMonitor.CellChangedEntry.Handler>) cellChangedEntry.handlers);
            foreach (CellChangeMonitor.CellChangedEntry.Handler handler1 in this.cellChangedCallbacksToRun)
            {
              foreach (CellChangeMonitor.CellChangedEntry.Handler handler2 in cellChangedEntry.handlers)
              {
                if (handler2.callback == handler1.callback)
                {
                  handler2.callback();
                  break;
                }
              }
            }
            this.transformLastKnownCell[dirtyTransform] = cell;
          }
        }
        else
          continue;
      }
      this.movingTransforms.Add(dirtyTransform);
      if (!this.previouslyMovingTransforms.Contains(dirtyTransform))
        this.RunMovementStateChangedCallbacks(dirtyTransform, true);
    }
    foreach (int previouslyMovingTransform in this.previouslyMovingTransforms)
    {
      if (!this.movingTransforms.Contains(previouslyMovingTransform))
        this.RunMovementStateChangedCallbacks(previouslyMovingTransform, false);
    }
    this.dirtyTransforms.Clear();
  }

  private void RunMovementStateChangedCallbacks(int instance_id, bool state)
  {
    CellChangeMonitor.MovementStateChangedEntry stateChangedEntry = new CellChangeMonitor.MovementStateChangedEntry();
    if (!this.movementStateChangedHandlers.TryGetValue(instance_id, out stateChangedEntry))
      return;
    this.moveChangedCallbacksToRun.Clear();
    this.moveChangedCallbacksToRun.AddRange((IEnumerable<System.Action<Transform, bool>>) stateChangedEntry.handlers);
    foreach (System.Action<Transform, bool> action in this.moveChangedCallbacksToRun)
    {
      if (stateChangedEntry.handlers.Contains(action))
        action(stateChangedEntry.transform, state);
    }
  }

  private void Validate()
  {
  }

  private struct CellChangedEntry
  {
    public Transform transform;
    public List<CellChangeMonitor.CellChangedEntry.Handler> handlers;

    public struct Handler
    {
      public string name;
      public System.Action callback;
    }
  }

  private struct MovementStateChangedEntry
  {
    public Transform transform;
    public List<System.Action<Transform, bool>> handlers;
  }
}
