// Decompiled with JetBrains decompiler
// Type: CameraSaveData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class CameraSaveData
{
  public static bool valid;
  public static Vector3 position;
  public static Vector3 localScale;
  public static Quaternion rotation;
  public static float orthographicsSize;

  public static void Load(FastReader reader)
  {
    CameraSaveData.position = reader.ReadVector3();
    CameraSaveData.localScale = reader.ReadVector3();
    CameraSaveData.rotation = reader.ReadQuaternion();
    CameraSaveData.orthographicsSize = reader.ReadSingle();
    CameraSaveData.valid = true;
  }
}
