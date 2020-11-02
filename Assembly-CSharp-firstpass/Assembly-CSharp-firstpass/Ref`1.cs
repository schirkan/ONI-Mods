// Decompiled with JetBrains decompiler
// Type: Ref`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{id}")]
public class Ref<ReferenceType> : ISaveLoadable where ReferenceType : KMonoBehaviour
{
  [Serialize]
  private int id = -1;
  private ReferenceType obj;

  public Ref(ReferenceType obj) => this.Set(obj);

  public Ref()
  {
  }

  private void UpdateID()
  {
    if ((bool) (Object) this.Get())
      this.id = this.obj.GetComponent<KPrefabID>().InstanceID;
    else
      this.id = -1;
  }

  [System.Runtime.Serialization.OnSerializing]
  public void OnSerializing() => this.UpdateID();

  public int GetId()
  {
    this.UpdateID();
    return this.id;
  }

  public ComponentType Get<ComponentType>() where ComponentType : MonoBehaviour
  {
    ReferenceType referenceType = this.Get();
    return (Object) referenceType == (Object) null ? default (ComponentType) : referenceType.GetComponent<ComponentType>();
  }

  public ReferenceType Get()
  {
    if ((Object) this.obj == (Object) null && this.id != -1)
    {
      KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.id);
      if ((Object) instance != (Object) null)
      {
        this.obj = instance.GetComponent<ReferenceType>();
        if ((Object) this.obj == (Object) null)
        {
          this.id = -1;
          Debug.LogWarning((object) ("Missing " + typeof (ReferenceType).Name + " reference: " + (object) this.id));
        }
      }
      else
      {
        Debug.LogWarning((object) ("Missing KPrefabID reference: " + (object) this.id));
        this.id = -1;
      }
    }
    return this.obj;
  }

  public void Set(ReferenceType obj)
  {
    this.id = !((Object) obj == (Object) null) ? obj.GetComponent<KPrefabID>().InstanceID : -1;
    this.obj = obj;
  }
}
