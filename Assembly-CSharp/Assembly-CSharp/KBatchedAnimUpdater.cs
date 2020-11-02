﻿// Decompiled with JetBrains decompiler
// Type: KBatchedAnimUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class KBatchedAnimUpdater : Singleton<KBatchedAnimUpdater>
{
  private const int VISIBLE_BORDER = 4;
  public static readonly Vector2I INVALID_CHUNK_ID = Vector2I.minusone;
  private Dictionary<int, KBatchedAnimController>[,] controllerGrid;
  private LinkedList<KBatchedAnimController> updateList = new LinkedList<KBatchedAnimController>();
  private LinkedList<KBatchedAnimController> alwaysUpdateList = new LinkedList<KBatchedAnimController>();
  private bool[,] visibleChunkGrid;
  private bool[,] previouslyVisibleChunkGrid;
  private List<Vector2I> visibleChunks = new List<Vector2I>();
  private List<Vector2I> previouslyVisibleChunks = new List<Vector2I>();
  private Vector2I vis_chunk_min = Vector2I.zero;
  private Vector2I vis_chunk_max = Vector2I.zero;
  private List<KBatchedAnimUpdater.RegistrationInfo> queuedRegistrations = new List<KBatchedAnimUpdater.RegistrationInfo>();
  private Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo> controllerChunkInfos = new Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo>();
  private Dictionary<int, KBatchedAnimUpdater.MovingControllerInfo> movingControllerInfos = new Dictionary<int, KBatchedAnimUpdater.MovingControllerInfo>();
  private const int CHUNKS_TO_CLEAN_PER_TICK = 16;
  private int cleanUpChunkIndex;
  private static readonly Vector2 VISIBLE_RANGE_SCALE = new Vector2(1.5f, 1.5f);

  public void InitializeGrid()
  {
    this.Clear();
    Vector2I visibleSize = this.GetVisibleSize();
    int length1 = (visibleSize.x + 32 - 1) / 32;
    int length2 = (visibleSize.y + 32 - 1) / 32;
    this.controllerGrid = new Dictionary<int, KBatchedAnimController>[length1, length2];
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < length1; ++index2)
        this.controllerGrid[index2, index1] = new Dictionary<int, KBatchedAnimController>();
    }
    this.visibleChunks.Clear();
    this.previouslyVisibleChunks.Clear();
    this.previouslyVisibleChunkGrid = new bool[length1, length2];
    this.visibleChunkGrid = new bool[length1, length2];
  }

  public Vector2I GetVisibleSize() => new Vector2I((int) ((double) Grid.WidthInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x), (int) ((double) Grid.HeightInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y));

  public void Clear()
  {
    foreach (KBatchedAnimController update in this.updateList)
    {
      if ((UnityEngine.Object) update != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) update);
    }
    this.updateList.Clear();
    foreach (KBatchedAnimController alwaysUpdate in this.alwaysUpdateList)
    {
      if ((UnityEngine.Object) alwaysUpdate != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) alwaysUpdate);
    }
    this.alwaysUpdateList.Clear();
    this.queuedRegistrations.Clear();
    this.visibleChunks.Clear();
    this.previouslyVisibleChunks.Clear();
    this.controllerGrid = (Dictionary<int, KBatchedAnimController>[,]) null;
    this.previouslyVisibleChunkGrid = (bool[,]) null;
    this.visibleChunkGrid = (bool[,]) null;
  }

  public void UpdateRegister(KBatchedAnimController controller)
  {
    switch (controller.updateRegistrationState)
    {
      case KBatchedAnimUpdater.RegistrationState.PendingRemoval:
        controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
        break;
      case KBatchedAnimUpdater.RegistrationState.Unregistered:
        (controller.visibilityType == KAnimControllerBase.VisibilityType.Always ? this.alwaysUpdateList : this.updateList).AddLast(controller);
        controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
        break;
    }
  }

  public void UpdateUnregister(KBatchedAnimController controller)
  {
    switch (controller.updateRegistrationState)
    {
      case KBatchedAnimUpdater.RegistrationState.Registered:
        controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.PendingRemoval;
        break;
    }
  }

  public void VisibilityRegister(KBatchedAnimController controller) => this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo()
  {
    transformId = controller.transform.GetInstanceID(),
    controllerInstanceId = controller.GetInstanceID(),
    controller = controller,
    register = true
  });

  public void VisibilityUnregister(KBatchedAnimController controller)
  {
    if (App.IsExiting)
      return;
    this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo()
    {
      transformId = controller.transform.GetInstanceID(),
      controllerInstanceId = controller.GetInstanceID(),
      controller = controller,
      register = false
    });
  }

  private Dictionary<int, KBatchedAnimController> GetControllerMap(
    Vector2I chunk_xy)
  {
    Dictionary<int, KBatchedAnimController> dictionary = (Dictionary<int, KBatchedAnimController>) null;
    if (this.controllerGrid != null && 0 <= chunk_xy.x && (chunk_xy.x < this.controllerGrid.GetLength(0) && 0 <= chunk_xy.y) && chunk_xy.y < this.controllerGrid.GetLength(1))
      dictionary = this.controllerGrid[chunk_xy.x, chunk_xy.y];
    return dictionary;
  }

  public void LateUpdate()
  {
    this.ProcessMovingAnims();
    this.UpdateVisibility();
    this.ProcessRegistrations();
    this.CleanUp();
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    int count1 = this.alwaysUpdateList.Count;
    KBatchedAnimUpdater.UpdateRegisteredAnims(this.alwaysUpdateList, unscaledDeltaTime);
    if (!this.DoGridProcessing())
      return;
    float deltaTime = Time.deltaTime;
    if ((double) deltaTime <= 0.0)
      return;
    int count2 = this.updateList.Count;
    KBatchedAnimUpdater.UpdateRegisteredAnims(this.updateList, deltaTime);
  }

  private static void UpdateRegisteredAnims(LinkedList<KBatchedAnimController> list, float dt)
  {
    LinkedListNode<KBatchedAnimController> next;
    for (LinkedListNode<KBatchedAnimController> node = list.First; node != null; node = next)
    {
      next = node.Next;
      KBatchedAnimController kbatchedAnimController = node.Value;
      if ((UnityEngine.Object) kbatchedAnimController == (UnityEngine.Object) null)
        list.Remove(node);
      else if (kbatchedAnimController.updateRegistrationState != KBatchedAnimUpdater.RegistrationState.Registered)
      {
        kbatchedAnimController.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;
        list.Remove(node);
      }
      else
        kbatchedAnimController.UpdateAnim(dt);
    }
  }

  public bool IsChunkVisible(Vector2I chunk_xy) => this.visibleChunkGrid[chunk_xy.x, chunk_xy.y];

  public void GetVisibleArea(out Vector2I vis_chunk_min, out Vector2I vis_chunk_max)
  {
    vis_chunk_min = this.vis_chunk_min;
    vis_chunk_max = this.vis_chunk_max;
  }

  public static Vector2I PosToChunkXY(Vector3 pos) => KAnimBatchManager.CellXYToChunkXY(Grid.PosToXY(pos));

  private void UpdateVisibility()
  {
    if (!this.DoGridProcessing())
      return;
    Vector2I min;
    Vector2I max;
    KBatchedAnimUpdater.GetVisibleCellRange(out min, out max);
    this.vis_chunk_min = new Vector2I(min.x / 32, min.y / 32);
    this.vis_chunk_max = new Vector2I(max.x / 32, max.y / 32);
    this.vis_chunk_max.x = Math.Min(this.vis_chunk_max.x, this.controllerGrid.GetLength(0) - 1);
    this.vis_chunk_max.y = Math.Min(this.vis_chunk_max.y, this.controllerGrid.GetLength(1) - 1);
    bool[,] visibleChunkGrid = this.previouslyVisibleChunkGrid;
    this.previouslyVisibleChunkGrid = this.visibleChunkGrid;
    this.visibleChunkGrid = visibleChunkGrid;
    Array.Clear((Array) this.visibleChunkGrid, 0, this.visibleChunkGrid.Length);
    List<Vector2I> previouslyVisibleChunks = this.previouslyVisibleChunks;
    this.previouslyVisibleChunks = this.visibleChunks;
    this.visibleChunks = previouslyVisibleChunks;
    this.visibleChunks.Clear();
    for (int y = this.vis_chunk_min.y; y <= this.vis_chunk_max.y; ++y)
    {
      for (int x = this.vis_chunk_min.x; x <= this.vis_chunk_max.x; ++x)
      {
        this.visibleChunkGrid[x, y] = true;
        this.visibleChunks.Add(new Vector2I(x, y));
        if (!this.previouslyVisibleChunkGrid[x, y])
        {
          foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair in this.controllerGrid[x, y])
          {
            KBatchedAnimController kbatchedAnimController = keyValuePair.Value;
            if (!((UnityEngine.Object) kbatchedAnimController == (UnityEngine.Object) null))
              kbatchedAnimController.SetVisiblity(true);
          }
        }
      }
    }
    for (int index = 0; index < this.previouslyVisibleChunks.Count; ++index)
    {
      Vector2I previouslyVisibleChunk = this.previouslyVisibleChunks[index];
      if (!this.visibleChunkGrid[previouslyVisibleChunk.x, previouslyVisibleChunk.y])
      {
        foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair in this.controllerGrid[previouslyVisibleChunk.x, previouslyVisibleChunk.y])
        {
          KBatchedAnimController kbatchedAnimController = keyValuePair.Value;
          if (!((UnityEngine.Object) kbatchedAnimController == (UnityEngine.Object) null))
            kbatchedAnimController.SetVisiblity(false);
        }
      }
    }
  }

  private void ProcessMovingAnims()
  {
    foreach (KBatchedAnimUpdater.MovingControllerInfo movingControllerInfo in this.movingControllerInfos.Values)
    {
      if (!((UnityEngine.Object) movingControllerInfo.controller == (UnityEngine.Object) null))
      {
        Vector2I chunkXy = KBatchedAnimUpdater.PosToChunkXY(movingControllerInfo.controller.PositionIncludingOffset);
        if (movingControllerInfo.chunkXY != chunkXy)
        {
          KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo();
          DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(movingControllerInfo.controllerInstanceId, out controllerChunkInfo));
          DebugUtil.Assert((UnityEngine.Object) movingControllerInfo.controller == (UnityEngine.Object) controllerChunkInfo.controller);
          DebugUtil.Assert(controllerChunkInfo.chunkXY == movingControllerInfo.chunkXY);
          Dictionary<int, KBatchedAnimController> controllerMap1 = this.GetControllerMap(controllerChunkInfo.chunkXY);
          if (controllerMap1 != null)
          {
            DebugUtil.Assert(controllerMap1.ContainsKey(movingControllerInfo.controllerInstanceId));
            controllerMap1.Remove(movingControllerInfo.controllerInstanceId);
          }
          Dictionary<int, KBatchedAnimController> controllerMap2 = this.GetControllerMap(chunkXy);
          if (controllerMap2 != null)
          {
            DebugUtil.Assert(!controllerMap2.ContainsKey(movingControllerInfo.controllerInstanceId));
            controllerMap2[movingControllerInfo.controllerInstanceId] = controllerChunkInfo.controller;
          }
          movingControllerInfo.chunkXY = chunkXy;
          controllerChunkInfo.chunkXY = chunkXy;
          this.controllerChunkInfos[movingControllerInfo.controllerInstanceId] = controllerChunkInfo;
          if (controllerMap2 != null)
            controllerChunkInfo.controller.SetVisiblity(this.visibleChunkGrid[chunkXy.x, chunkXy.y]);
          else
            controllerChunkInfo.controller.SetVisiblity(false);
        }
      }
    }
  }

  private void ProcessRegistrations()
  {
    for (int index = 0; index < this.queuedRegistrations.Count; ++index)
    {
      KBatchedAnimUpdater.RegistrationInfo queuedRegistration = this.queuedRegistrations[index];
      if (queuedRegistration.register)
      {
        if (!((UnityEngine.Object) queuedRegistration.controller == (UnityEngine.Object) null))
        {
          int instanceId = queuedRegistration.controller.GetInstanceID();
          DebugUtil.Assert(!this.controllerChunkInfos.ContainsKey(instanceId));
          KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo()
          {
            controller = queuedRegistration.controller,
            chunkXY = KBatchedAnimUpdater.PosToChunkXY(queuedRegistration.controller.PositionIncludingOffset)
          };
          this.controllerChunkInfos[instanceId] = controllerChunkInfo;
          Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(queuedRegistration.controller.transform, new System.Action<Transform, bool>(this.OnMovementStateChanged));
          Dictionary<int, KBatchedAnimController> controllerMap = this.GetControllerMap(controllerChunkInfo.chunkXY);
          if (controllerMap != null)
          {
            DebugUtil.Assert(!controllerMap.ContainsKey(instanceId));
            controllerMap.Add(instanceId, queuedRegistration.controller);
          }
          if (Singleton<CellChangeMonitor>.Instance.IsMoving(queuedRegistration.controller.transform))
          {
            DebugUtil.DevAssertArgs((!this.movingControllerInfos.ContainsKey(instanceId) ? 1 : 0) != 0, (object) "Readding controller which is already moving", (object) queuedRegistration.controller.name, (object) controllerChunkInfo.chunkXY, this.movingControllerInfos.ContainsKey(instanceId) ? (object) this.movingControllerInfos[instanceId].chunkXY.ToString() : (object) (string) null);
            this.movingControllerInfos[instanceId] = new KBatchedAnimUpdater.MovingControllerInfo()
            {
              controllerInstanceId = instanceId,
              controller = queuedRegistration.controller,
              chunkXY = controllerChunkInfo.chunkXY
            };
          }
          if (controllerMap != null && this.visibleChunkGrid[controllerChunkInfo.chunkXY.x, controllerChunkInfo.chunkXY.y])
            queuedRegistration.controller.SetVisiblity(true);
        }
      }
      else
      {
        KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo();
        if (this.controllerChunkInfos.TryGetValue(queuedRegistration.controllerInstanceId, out controllerChunkInfo))
        {
          if ((UnityEngine.Object) queuedRegistration.controller != (UnityEngine.Object) null)
          {
            Dictionary<int, KBatchedAnimController> controllerMap = this.GetControllerMap(controllerChunkInfo.chunkXY);
            if (controllerMap != null)
            {
              DebugUtil.Assert(controllerMap.ContainsKey(queuedRegistration.controllerInstanceId));
              controllerMap.Remove(queuedRegistration.controllerInstanceId);
            }
            queuedRegistration.controller.SetVisiblity(false);
          }
          this.movingControllerInfos.Remove(queuedRegistration.controllerInstanceId);
          Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(queuedRegistration.transformId, new System.Action<Transform, bool>(this.OnMovementStateChanged));
          this.controllerChunkInfos.Remove(queuedRegistration.controllerInstanceId);
        }
      }
    }
    this.queuedRegistrations.Clear();
  }

  public void OnMovementStateChanged(Transform transform, bool is_moving)
  {
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      return;
    KBatchedAnimController component = transform.GetComponent<KBatchedAnimController>();
    int instanceId = component.GetInstanceID();
    KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo();
    DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(instanceId, out controllerChunkInfo));
    if (is_moving)
    {
      DebugUtil.DevAssertArgs((!this.movingControllerInfos.ContainsKey(instanceId) ? 1 : 0) != 0, (object) "Readding controller which is already moving", (object) component.name, (object) controllerChunkInfo.chunkXY, this.movingControllerInfos.ContainsKey(instanceId) ? (object) this.movingControllerInfos[instanceId].chunkXY.ToString() : (object) (string) null);
      this.movingControllerInfos[instanceId] = new KBatchedAnimUpdater.MovingControllerInfo()
      {
        controllerInstanceId = instanceId,
        controller = component,
        chunkXY = controllerChunkInfo.chunkXY
      };
    }
    else
      this.movingControllerInfos.Remove(instanceId);
  }

  private void CleanUp()
  {
    if (!this.DoGridProcessing())
      return;
    int length = this.controllerGrid.GetLength(0);
    for (int index = 0; index < 16; ++index)
    {
      int num = (this.cleanUpChunkIndex + index) % this.controllerGrid.Length;
      Dictionary<int, KBatchedAnimController> dictionary = this.controllerGrid[num % length, num / length];
      ListPool<int, KBatchedAnimUpdater>.PooledList pooledList = ListPool<int, KBatchedAnimUpdater>.Allocate();
      foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair in dictionary)
      {
        if ((UnityEngine.Object) keyValuePair.Value == (UnityEngine.Object) null)
          pooledList.Add(keyValuePair.Key);
      }
      foreach (int key in (List<int>) pooledList)
        dictionary.Remove(key);
      pooledList.Recycle();
    }
    this.cleanUpChunkIndex = (this.cleanUpChunkIndex + 16) % this.controllerGrid.Length;
  }

  public static void GetVisibleCellRange(out Vector2I min, out Vector2I max)
  {
    Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
    min.x -= 4;
    min.y -= 4;
    min.x = Math.Min((int) ((double) Grid.WidthInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x) - 1, Math.Max(0, min.x));
    min.y = Math.Min((int) ((double) Grid.HeightInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y) - 1, Math.Max(0, min.y));
    max.x += 4;
    max.y += 4;
    max.x = Math.Min((int) ((double) Grid.WidthInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x) - 1, Math.Max(0, max.x));
    max.y = Math.Min((int) ((double) Grid.HeightInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y) - 1, Math.Max(0, max.y));
  }

  private bool DoGridProcessing() => this.controllerGrid != null && (UnityEngine.Object) Camera.main != (UnityEngine.Object) null;

  public enum RegistrationState
  {
    Registered,
    PendingRemoval,
    Unregistered,
  }

  private struct RegistrationInfo
  {
    public bool register;
    public int transformId;
    public int controllerInstanceId;
    public KBatchedAnimController controller;
  }

  private struct ControllerChunkInfo
  {
    public KBatchedAnimController controller;
    public Vector2I chunkXY;
  }

  private class MovingControllerInfo
  {
    public int controllerInstanceId;
    public KBatchedAnimController controller;
    public Vector2I chunkXY;
  }
}
