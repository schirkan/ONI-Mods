// Decompiled with JetBrains decompiler
// Type: FMODUnity.RuntimeUtils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD;
using FMOD.Studio;
using System;
using System.IO;
using UnityEngine;

namespace FMODUnity
{
  public static class RuntimeUtils
  {
    public const string LogFileName = "fmod.log";
    private const string BankExtension = ".bank";

    public static VECTOR ToFMODVector(this Vector3 vec)
    {
      VECTOR vector;
      vector.x = vec.x;
      vector.y = vec.y;
      vector.z = vec.z;
      return vector;
    }

    public static ATTRIBUTES_3D To3DAttributes(this Vector3 pos) => new ATTRIBUTES_3D()
    {
      forward = Vector3.forward.ToFMODVector(),
      up = Vector3.up.ToFMODVector(),
      position = pos.ToFMODVector()
    };

    public static ATTRIBUTES_3D To3DAttributes(this Transform transform) => new ATTRIBUTES_3D()
    {
      forward = transform.forward.ToFMODVector(),
      up = transform.up.ToFMODVector(),
      position = transform.position.ToFMODVector()
    };

    public static ATTRIBUTES_3D To3DAttributes(
      Transform transform,
      Rigidbody rigidbody = null)
    {
      ATTRIBUTES_3D attributes3D = transform.To3DAttributes();
      if ((bool) (UnityEngine.Object) rigidbody)
        attributes3D.velocity = rigidbody.velocity.ToFMODVector();
      return attributes3D;
    }

    public static ATTRIBUTES_3D To3DAttributes(GameObject go, Rigidbody rigidbody = null)
    {
      ATTRIBUTES_3D attributes3D = go.transform.To3DAttributes();
      if ((bool) (UnityEngine.Object) rigidbody)
        attributes3D.velocity = rigidbody.velocity.ToFMODVector();
      return attributes3D;
    }

    public static ATTRIBUTES_3D To3DAttributes(
      Transform transform,
      Rigidbody2D rigidbody)
    {
      ATTRIBUTES_3D attributes3D = transform.To3DAttributes();
      if ((bool) (UnityEngine.Object) rigidbody)
      {
        VECTOR vector;
        vector.x = rigidbody.velocity.x;
        vector.y = rigidbody.velocity.y;
        vector.z = 0.0f;
        attributes3D.velocity = vector;
      }
      return attributes3D;
    }

    public static ATTRIBUTES_3D To3DAttributes(GameObject go, Rigidbody2D rigidbody)
    {
      ATTRIBUTES_3D attributes3D = go.transform.To3DAttributes();
      if ((bool) (UnityEngine.Object) rigidbody)
      {
        VECTOR vector;
        vector.x = rigidbody.velocity.x;
        vector.y = rigidbody.velocity.y;
        vector.z = 0.0f;
        attributes3D.velocity = vector;
      }
      return attributes3D;
    }

    internal static FMODPlatform GetCurrentPlatform() => FMODPlatform.Windows;

    internal static string GetBankPath(string bankName)
    {
      string streamingAssetsPath = Application.streamingAssetsPath;
      return Path.GetExtension(bankName) != ".bank" ? string.Format("{0}/{1}.bank", (object) streamingAssetsPath, (object) bankName) : string.Format("{0}/{1}", (object) streamingAssetsPath, (object) bankName);
    }

    internal static string GetPluginPath(string pluginName) => Application.dataPath + "/Plugins/" + (pluginName + ".dll");

    public static void EnforceLibraryOrder()
    {
      int stats = (int) Memory.GetStats(out int _, out int _);
      int id = (int) Util.ParseID("", out Guid _);
    }
  }
}
