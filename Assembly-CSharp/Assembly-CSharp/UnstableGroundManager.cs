// Decompiled with JetBrains decompiler
// Type: UnstableGroundManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGroundManager")]
public class UnstableGroundManager : KMonoBehaviour
{
  [SerializeField]
  private Vector3 spawnPuffOffset;
  [SerializeField]
  private Vector3 landEffectOffset;
  [SerializeField]
  private UnstableGroundManager.EffectInfo[] effects;
  private List<GameObject> fallingObjects = new List<GameObject>();
  private List<int> pendingCells = new List<int>();
  private Dictionary<SimHashes, UnstableGroundManager.EffectRuntimeInfo> runtimeInfo = new Dictionary<SimHashes, UnstableGroundManager.EffectRuntimeInfo>();
  [Serialize]
  private List<UnstableGroundManager.SerializedInfo> serializedInfo;

  protected override void OnPrefabInit()
  {
    foreach (UnstableGroundManager.EffectInfo effect in this.effects)
    {
      GameObject prefab = effect.prefab;
      prefab.SetActive(false);
      UnstableGroundManager.EffectRuntimeInfo effectRuntimeInfo = new UnstableGroundManager.EffectRuntimeInfo();
      ObjectPool pool = new ObjectPool((Func<GameObject>) (() => this.InstantiateObj(prefab)), 16);
      effectRuntimeInfo.pool = pool;
      effectRuntimeInfo.releaseFunc = (System.Action<GameObject>) (go =>
      {
        this.ReleaseGO(go);
        pool.ReleaseInstance(go);
      });
      this.runtimeInfo[effect.element] = effectRuntimeInfo;
    }
  }

  private void ReleaseGO(GameObject go)
  {
    if (GameComps.Gravities.Has((object) go))
      GameComps.Gravities.Remove(go);
    go.SetActive(false);
  }

  private GameObject InstantiateObj(GameObject prefab)
  {
    GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.SceneLayer.BuildingBack);
    gameObject.SetActive(false);
    gameObject.name = "UnstablePool";
    return gameObject;
  }

  public void Spawn(
    int cell,
    Element element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count)
  {
    Vector3 posCcc = Grid.CellToPosCCC(cell, Grid.SceneLayer.TileMain);
    if (float.IsNaN(temperature) || float.IsInfinity(temperature))
    {
      Debug.LogError((object) "Tried to spawn unstable ground with NaN temperature");
      temperature = 293f;
    }
    KBatchedAnimController kbatchedAnimController = this.Spawn(posCcc, element, mass, temperature, disease_idx, disease_count);
    kbatchedAnimController.Play((HashedString) "start");
    kbatchedAnimController.Play((HashedString) "loop", KAnim.PlayMode.Loop);
    kbatchedAnimController.gameObject.name = "Falling " + element.name;
    GameComps.Gravities.Add(kbatchedAnimController.gameObject, Vector2.zero);
    this.fallingObjects.Add(kbatchedAnimController.gameObject);
    this.SpawnPuff(posCcc, element, mass, temperature, disease_idx, disease_count);
    Substance substance = element.substance;
    if (substance == null || substance.fallingStartSound == null || !CameraController.Instance.IsAudibleSound(posCcc, (HashedString) substance.fallingStartSound))
      return;
    SoundEvent.PlayOneShot(substance.fallingStartSound, posCcc);
  }

  private void SpawnOld(
    Vector3 pos,
    Element element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count)
  {
    if (!element.IsUnstable)
      Debug.LogError((object) "Spawning falling ground with a stable element");
    KBatchedAnimController kbatchedAnimController = this.Spawn(pos, element, mass, temperature, disease_idx, disease_count);
    GameComps.Gravities.Add(kbatchedAnimController.gameObject, Vector2.zero);
    kbatchedAnimController.Play((HashedString) "loop", KAnim.PlayMode.Loop);
    this.fallingObjects.Add(kbatchedAnimController.gameObject);
    kbatchedAnimController.gameObject.name = "SpawnOld " + element.name;
  }

  private void SpawnPuff(
    Vector3 pos,
    Element element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count)
  {
    if (!element.IsUnstable)
      Debug.LogError((object) "Spawning sand puff with a stable element");
    KBatchedAnimController kbatchedAnimController = this.Spawn(pos, element, mass, temperature, disease_idx, disease_count);
    kbatchedAnimController.Play((HashedString) "sandPuff");
    kbatchedAnimController.gameObject.name = "Puff " + element.name;
    kbatchedAnimController.transform.SetPosition(kbatchedAnimController.transform.GetPosition() + this.spawnPuffOffset);
  }

  private KBatchedAnimController Spawn(
    Vector3 pos,
    Element element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count)
  {
    UnstableGroundManager.EffectRuntimeInfo effectRuntimeInfo;
    if (!this.runtimeInfo.TryGetValue(element.id, out effectRuntimeInfo))
      Debug.LogError((object) (element.id.ToString() + " needs unstable ground info hookup!"));
    GameObject instance = effectRuntimeInfo.pool.GetInstance();
    instance.transform.SetPosition(pos);
    if (float.IsNaN(temperature) || float.IsInfinity(temperature))
    {
      Debug.LogError((object) "Tried to spawn unstable ground with NaN temperature");
      temperature = 293f;
    }
    UnstableGround component1 = instance.GetComponent<UnstableGround>();
    component1.element = element.id;
    component1.mass = mass;
    component1.temperature = temperature;
    component1.diseaseIdx = disease_idx;
    component1.diseaseCount = disease_count;
    instance.SetActive(true);
    KBatchedAnimController component2 = instance.GetComponent<KBatchedAnimController>();
    component2.onDestroySelf = effectRuntimeInfo.releaseFunc;
    component2.Stop();
    if (element.substance != null)
      component2.TintColour = element.substance.colour;
    return component2;
  }

  public List<int> GetCellsContainingFallingAbove(Vector2I cellXY)
  {
    List<int> intList = new List<int>();
    for (int index = 0; index < this.fallingObjects.Count; ++index)
    {
      Vector2I xy;
      Grid.PosToXY(this.fallingObjects[index].transform.GetPosition(), out xy);
      if (xy.x == cellXY.x || xy.y >= cellXY.y)
      {
        int cell = Grid.PosToCell((Vector2) xy);
        intList.Add(cell);
      }
    }
    for (int index = 0; index < this.pendingCells.Count; ++index)
    {
      Vector2I xy = Grid.CellToXY(this.pendingCells[index]);
      if (xy.x == cellXY.x || xy.y >= cellXY.y)
        intList.Add(this.pendingCells[index]);
    }
    return intList;
  }

  private void RemoveFromPending(int cell) => this.pendingCells.Remove(cell);

  private void Update()
  {
    if (App.isLoading)
      return;
    int index1 = 0;
    while (index1 < this.fallingObjects.Count)
    {
      GameObject fallingObject = this.fallingObjects[index1];
      if (!((UnityEngine.Object) fallingObject == (UnityEngine.Object) null))
      {
        Vector3 position = fallingObject.transform.GetPosition();
        int cell = Grid.PosToCell(position);
        int index2 = Grid.CellBelow(cell);
        if (!Grid.IsValidCell(index2) || Grid.Element[index2].IsSolid || ((int) Grid.Properties[index2] & 4) != 0)
        {
          UnstableGround component = fallingObject.GetComponent<UnstableGround>();
          this.pendingCells.Add(cell);
          HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() => this.RemoveFromPending(cell))));
          SimMessages.AddRemoveSubstance(cell, component.element, CellEventLogger.Instance.UnstableGround, component.mass, component.temperature, component.diseaseIdx, component.diseaseCount, callbackIdx: handle.index);
          ListPool<ScenePartitionerEntry, UnstableGroundManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, UnstableGroundManager>.Allocate();
          Vector2I xy = Grid.CellToXY(cell);
          xy.x = Mathf.Max(0, xy.x - 1);
          xy.y = Mathf.Min(Grid.HeightInCells - 1, xy.y + 1);
          GameScenePartitioner.Instance.GatherEntries(xy.x, xy.y, 3, 3, GameScenePartitioner.Instance.collisionLayer, (List<ScenePartitionerEntry>) pooledList);
          foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) pooledList)
          {
            if (partitionerEntry.obj is KCollider2D)
              (partitionerEntry.obj as KCollider2D).gameObject.Trigger(-975551167);
          }
          pooledList.Recycle();
          Element elementByHash = ElementLoader.FindElementByHash(component.element);
          if (elementByHash != null && elementByHash.substance != null && (elementByHash.substance.fallingStopSound != null && CameraController.Instance.IsAudibleSound(position, (HashedString) elementByHash.substance.fallingStopSound)))
            SoundEvent.PlayOneShot(elementByHash.substance.fallingStopSound, position);
          GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.OreAbsorbId), position + this.landEffectOffset, Grid.SceneLayer.Front).SetActive(true);
          this.fallingObjects[index1] = this.fallingObjects[this.fallingObjects.Count - 1];
          this.fallingObjects.RemoveAt(this.fallingObjects.Count - 1);
          this.ReleaseGO(fallingObject);
        }
        else
          ++index1;
      }
    }
  }

  [System.Runtime.Serialization.OnSerializing]
  private void OnSerializing()
  {
    if (this.fallingObjects.Count > 0)
      this.serializedInfo = new List<UnstableGroundManager.SerializedInfo>();
    foreach (GameObject fallingObject in this.fallingObjects)
    {
      UnstableGround component = fallingObject.GetComponent<UnstableGround>();
      byte diseaseIdx = component.diseaseIdx;
      int num = diseaseIdx != byte.MaxValue ? Db.Get().Diseases[(int) diseaseIdx].id.HashValue : 0;
      this.serializedInfo.Add(new UnstableGroundManager.SerializedInfo()
      {
        position = fallingObject.transform.GetPosition(),
        element = component.element,
        mass = component.mass,
        temperature = component.temperature,
        diseaseID = num,
        diseaseCount = component.diseaseCount
      });
    }
  }

  [System.Runtime.Serialization.OnSerialized]
  private void OnSerialized() => this.serializedInfo = (List<UnstableGroundManager.SerializedInfo>) null;

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (this.serializedInfo == null)
      return;
    this.fallingObjects.Clear();
    HashedString id = new HashedString();
    foreach (UnstableGroundManager.SerializedInfo serializedInfo in this.serializedInfo)
    {
      Element elementByHash = ElementLoader.FindElementByHash(serializedInfo.element);
      id.HashValue = serializedInfo.diseaseID;
      byte index = Db.Get().Diseases.GetIndex(id);
      int disease_count = serializedInfo.diseaseCount;
      if (index == byte.MaxValue)
        disease_count = 0;
      this.SpawnOld(serializedInfo.position, elementByHash, serializedInfo.mass, serializedInfo.temperature, index, disease_count);
    }
  }

  [Serializable]
  private struct EffectInfo
  {
    [HashedEnum]
    public SimHashes element;
    public GameObject prefab;
  }

  private struct EffectRuntimeInfo
  {
    public ObjectPool pool;
    public System.Action<GameObject> releaseFunc;
  }

  private struct SerializedInfo
  {
    public Vector3 position;
    public SimHashes element;
    public float mass;
    public float temperature;
    public int diseaseID;
    public int diseaseCount;
  }
}
