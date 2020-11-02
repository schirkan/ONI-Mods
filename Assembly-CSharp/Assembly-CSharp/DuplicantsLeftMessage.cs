// Decompiled with JetBrains decompiler
// Type: DuplicantsLeftMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class DuplicantsLeftMessage : Message
{
  public override string GetSound() => "";

  public override string GetTitle() => (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;

  public override string GetMessageBody() => (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;

  public override string GetTooltip() => (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
}
