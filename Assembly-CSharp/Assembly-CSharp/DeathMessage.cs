// Decompiled with JetBrains decompiler
// Type: DeathMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

public class DeathMessage : TargetMessage
{
  [Serialize]
  private ResourceRef<Death> death = new ResourceRef<Death>();

  public DeathMessage()
  {
  }

  public DeathMessage(GameObject go, Death death)
    : base(go.GetComponent<KPrefabID>())
    => this.death.Set(death);

  public override string GetSound() => "";

  public override bool PlayNotificationSound() => false;

  public override string GetTitle() => (string) MISC.NOTIFICATIONS.DUPLICANTDIED.NAME;

  public override string GetTooltip() => this.GetMessageBody();

  public override string GetMessageBody() => this.death.Get().description.Replace("{Target}", this.GetTarget().GetName());
}
