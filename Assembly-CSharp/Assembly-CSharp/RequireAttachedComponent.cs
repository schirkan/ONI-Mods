// Decompiled with JetBrains decompiler
// Type: RequireAttachedComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : RocketLaunchCondition
{
  private string typeNameString;
  private System.Type requiredType;
  private AttachableBuilding myAttachable;

  public System.Type RequiredType
  {
    get => this.requiredType;
    set
    {
      this.requiredType = value;
      this.typeNameString = this.requiredType.Name;
    }
  }

  public RequireAttachedComponent(
    AttachableBuilding myAttachable,
    System.Type required_type,
    string type_name_string)
  {
    this.myAttachable = myAttachable;
    this.requiredType = required_type;
    this.typeNameString = type_name_string;
  }

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    if ((UnityEngine.Object) this.myAttachable != (UnityEngine.Object) null)
    {
      foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.myAttachable))
      {
        if ((bool) (UnityEngine.Object) gameObject.GetComponent(this.requiredType))
          return RocketLaunchCondition.LaunchStatus.Ready;
      }
    }
    return RocketLaunchCondition.LaunchStatus.Failure;
  }

  public override string GetLaunchStatusMessage(bool ready) => ready ? this.typeNameString + " " + (string) UI.STARMAP.LAUNCHCHECKLIST.REQUIRED : this.typeNameString + " " + (string) UI.STARMAP.LAUNCHCHECKLIST.INSTALLED;

  public override string GetLaunchStatusTooltip(bool ready) => ready ? string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, (object) this.typeNameString) : string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.REQUIRED_TOOLTIP, (object) this.typeNameString);
}
