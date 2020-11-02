// Decompiled with JetBrains decompiler
// Type: AttackableBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/AttackableBase")]
public class AttackableBase : Workable, IApproachable
{
  private HandleVector<int>.Handle scenePartitionerEntry;
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDeadTagChangedDelegate = GameUtil.CreateHasTagHandler<AttackableBase>(GameTags.Dead, (System.Action<AttackableBase, object>) ((component, data) => component.OnDefeated(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((System.Action<AttackableBase, object>) ((component, data) => component.OnDefeated(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> SetupScenePartitionerDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((System.Action<AttackableBase, object>) ((component, data) => component.SetupScenePartitioner(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnCellChangedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((System.Action<AttackableBase, object>) ((component, data) => GameScenePartitioner.Instance.UpdatePosition(component.scenePartitionerEntry, Grid.PosToCell(component.gameObject))));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.attributeConverter = Db.Get().AttributeConverters.AttackDamage;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
    this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
    this.SetupScenePartitioner();
    this.Subscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
    GameUtil.SubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagChangedDelegate);
    this.Subscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
    this.Subscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
  }

  public float GetDamageMultiplier() => this.attributeConverter != null && (UnityEngine.Object) this.worker != (UnityEngine.Object) null ? Mathf.Max(1f + this.worker.GetComponent<AttributeConverters>().GetConverter(this.attributeConverter.Id).Evaluate(), 0.1f) : 1f;

  private void SetupScenePartitioner(object data = null)
  {
    Extents extents = new Extents(Grid.PosToXY(this.transform.GetPosition()).x, Grid.PosToXY(this.transform.GetPosition()).y, 1, 1);
    this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(this.gameObject.name, (object) this.GetComponent<FactionAlignment>(), extents, GameScenePartitioner.Instance.attackableEntitiesLayer, (System.Action<object>) null);
  }

  private void OnDefeated(object data = null) => GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);

  public override float GetEfficiencyMultiplier(Worker worker) => 1f;

  protected override void OnCleanUp()
  {
    this.Unsubscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
    this.Unsubscribe<AttackableBase>(1623392196, AttackableBase.OnDefeatedDelegate);
    this.Unsubscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
    GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
    base.OnCleanUp();
  }
}
