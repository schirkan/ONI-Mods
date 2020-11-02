// Decompiled with JetBrains decompiler
// Type: Audio
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class Audio : ScriptableObject
{
  private static Audio _Instance;
  public float listenerMinZ;
  public float listenerMinOrthographicSize;
  public float listenerReferenceZ;
  public float listenerReferenceOrthographicSize;

  public static Audio Get()
  {
    if ((Object) Audio._Instance == (Object) null)
      Audio._Instance = Resources.Load<Audio>(nameof (Audio));
    return Audio._Instance;
  }
}
