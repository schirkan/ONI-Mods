// Decompiled with JetBrains decompiler
// Type: Util
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;

public static class Util
{
  private static HashSet<char> defaultInvalidUserInputChars = new HashSet<char>((IEnumerable<char>) Path.GetInvalidPathChars());
  private static HashSet<char> additionalInvalidUserInputChars = new HashSet<char>((IEnumerable<char>) new char[9]
  {
    '<',
    '>',
    ':',
    '"',
    '/',
    '?',
    '*',
    '\\',
    '!'
  });
  private static System.Random random = new System.Random();
  private static string defaultRootFolder = Application.persistentDataPath;

  public static void Swap<T>(ref T a, ref T b)
  {
    T obj = a;
    a = b;
    b = obj;
  }

  public static void InitializeComponent(Component cmp)
  {
    if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
      return;
    KMonoBehaviour kmonoBehaviour = cmp as KMonoBehaviour;
    if (!((UnityEngine.Object) kmonoBehaviour != (UnityEngine.Object) null))
      return;
    kmonoBehaviour.InitializeComponent();
  }

  public static void SpawnComponent(Component cmp)
  {
    if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
      return;
    KMonoBehaviour kmonoBehaviour = cmp as KMonoBehaviour;
    if (!((UnityEngine.Object) kmonoBehaviour != (UnityEngine.Object) null))
      return;
    kmonoBehaviour.Spawn();
  }

  public static Component FindComponent(this Component cmp, string targetName) => cmp.gameObject.FindComponent(targetName);

  public static Component FindComponent(this GameObject go, string targetName)
  {
    Component component = go.GetComponent(targetName);
    Util.InitializeComponent(component);
    return component;
  }

  public static T FindComponent<T>(this Component c) where T : Component => c.gameObject.FindComponent<T>();

  public static T FindComponent<T>(this GameObject go) where T : Component
  {
    T component = go.GetComponent<T>();
    Util.InitializeComponent((Component) component);
    return component;
  }

  public static T FindOrAddUnityComponent<T>(this Component cmp) where T : Component => cmp.gameObject.FindOrAddUnityComponent<T>();

  public static T FindOrAddUnityComponent<T>(this GameObject go) where T : Component
  {
    T obj = go.GetComponent<T>();
    if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
      obj = go.AddComponent<T>();
    return obj;
  }

  public static Component RequireComponent(this Component cmp, string name) => cmp.gameObject.RequireComponent(name);

  public static Component RequireComponent(this GameObject go, string name)
  {
    Component component = go.GetComponent(name);
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogErrorFormat((UnityEngine.Object) go, "{0} '{1}' requires a component of type {2}!", (object) go.GetType().ToString(), (object) go.name, (object) name);
      return (Component) null;
    }
    Util.InitializeComponent(component);
    return component;
  }

  public static T RequireComponent<T>(this Component cmp) where T : Component
  {
    T component = cmp.gameObject.GetComponent<T>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogErrorFormat((UnityEngine.Object) cmp.gameObject, "{0} '{1}' requires a component of type {2} as requested by {3}!", (object) cmp.gameObject.GetType().ToString(), (object) cmp.gameObject.name, (object) typeof (T).ToString(), (object) cmp.GetType().ToString());
      return default (T);
    }
    Util.InitializeComponent((Component) component);
    return component;
  }

  public static T RequireComponent<T>(this GameObject gameObject) where T : Component
  {
    T component = gameObject.GetComponent<T>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogErrorFormat((UnityEngine.Object) gameObject, "{0} '{1}' requires a component of type {2}!", (object) gameObject.GetType().ToString(), (object) gameObject.name, (object) typeof (T).ToString());
      return default (T);
    }
    Util.InitializeComponent((Component) component);
    return component;
  }

  public static void SetLayerRecursively(this GameObject go, int layer) => Util.SetLayer(go.transform, layer);

  public static void SetLayer(Transform t, int layer)
  {
    t.gameObject.layer = layer;
    for (int index = 0; index < t.childCount; ++index)
      Util.SetLayer(t.GetChild(index), layer);
  }

  public static void KDestroyGameObject(Component original)
  {
    Debug.Assert((UnityEngine.Object) original != (UnityEngine.Object) null, (object) "Attempted to destroy a GameObject that is already destroyed.");
    Util.KDestroyGameObject(original.gameObject);
  }

  public static void KDestroyGameObject(GameObject original)
  {
    Debug.Assert((UnityEngine.Object) original != (UnityEngine.Object) null, (object) "Attempted to destroy a GameObject that is already destroyed.");
    original.DeleteObject();
  }

  public static T FindOrAddComponent<T>(this Component cmp) where T : Component => cmp.gameObject.FindOrAddComponent<T>();

  public static T FindOrAddComponent<T>(this GameObject go) where T : Component
  {
    T obj = go.GetComponent<T>();
    if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
    {
      obj = go.AddComponent<T>();
      KMonoBehaviour kmonoBehaviour = (object) obj as KMonoBehaviour;
      if ((UnityEngine.Object) kmonoBehaviour != (UnityEngine.Object) null && !KMonoBehaviour.isPoolPreInit && !kmonoBehaviour.IsInitialized())
        Debug.LogErrorFormat("Could not find component " + typeof (T).ToString() + " on object " + go.ToString(), (object[]) Array.Empty<object>());
    }
    else
      Util.InitializeComponent((Component) obj);
    return obj;
  }

  public static void PreInit(this GameObject go)
  {
    KMonoBehaviour.isPoolPreInit = true;
    foreach (KMonoBehaviour component in go.GetComponents<KMonoBehaviour>())
      component.InitializeComponent();
    KMonoBehaviour.isPoolPreInit = false;
  }

  public static GameObject KInstantiate(GameObject original, Vector3 position) => Util.KInstantiate(original, position, Quaternion.identity);

  public static GameObject KInstantiate(
    Component original,
    GameObject parent = null,
    string name = null)
  {
    return Util.KInstantiate(original.gameObject, Vector3.zero, Quaternion.identity, parent, name);
  }

  public static GameObject KInstantiate(
    GameObject original,
    GameObject parent = null,
    string name = null)
  {
    return Util.KInstantiate(original, Vector3.zero, Quaternion.identity, parent, name);
  }

  public static GameObject KInstantiate(
    GameObject original,
    Vector3 position,
    Quaternion rotation,
    GameObject parent = null,
    string name = null,
    bool initialize_id = true,
    int gameLayer = 0)
  {
    if (App.IsExiting)
      return (GameObject) null;
    GameObject go = (GameObject) null;
    if ((UnityEngine.Object) original == (UnityEngine.Object) null)
      DebugUtil.LogWarningArgs((object) "Missing prefab");
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) original.GetComponent<RectTransform>() != (UnityEngine.Object) null && (UnityEngine.Object) parent != (UnityEngine.Object) null)
      {
        go = UnityEngine.Object.Instantiate<GameObject>(original, position, rotation);
        go.transform.SetParent(parent.transform, true);
      }
      else
      {
        Transform parent1 = (Transform) null;
        if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
          parent1 = parent.transform;
        go = UnityEngine.Object.Instantiate<GameObject>(original, position, rotation, parent1);
      }
      if (gameLayer != 0)
        go.SetLayerRecursively(gameLayer);
    }
    if (name != null)
      go.name = name;
    else
      go.name = original.name;
    KPrefabID component1 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      if (initialize_id)
      {
        component1.InstanceID = KPrefabID.GetUniqueID();
        KPrefabIDTracker.Get().Register(component1);
      }
      KPrefabID component2 = original.GetComponent<KPrefabID>();
      component1.CopyTags(component2);
      component1.CopyInitFunctions(component2);
      component1.RunInstantiateFn();
    }
    return go;
  }

  public static T KInstantiateUI<T>(GameObject original, GameObject parent = null, bool force_active = false) where T : Component => Util.KInstantiateUI(original, parent, force_active).GetComponent<T>();

  public static GameObject KInstantiateUI(
    GameObject original,
    GameObject parent = null,
    bool force_active = false)
  {
    if (App.IsExiting)
      return (GameObject) null;
    GameObject gameObject = (GameObject) null;
    if ((UnityEngine.Object) original == (UnityEngine.Object) null)
      DebugUtil.LogWarningArgs((object) "Missing prefab");
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      gameObject = UnityEngine.Object.Instantiate<GameObject>(original, (UnityEngine.Object) parent != (UnityEngine.Object) null ? parent.transform : (Transform) null, false);
    gameObject.name = original.name;
    if (force_active)
      gameObject.SetActive(true);
    return gameObject;
  }

  public static GameObject NewGameObject(GameObject parent, string name)
  {
    GameObject gameObject = new GameObject();
    if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
      gameObject.transform.parent = parent.transform;
    if (name != null)
      gameObject.name = name;
    return gameObject;
  }

  public static T UpdateComponentRequirement<T>(this GameObject go, bool required = true) where T : Component
  {
    T obj = go.GetComponent(typeof (T)) as T;
    if (!required && (UnityEngine.Object) obj != (UnityEngine.Object) null)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) obj, true);
      obj = default (T);
    }
    else if (required && (UnityEngine.Object) obj == (UnityEngine.Object) null)
      obj = go.AddComponent(typeof (T)) as T;
    return obj;
  }

  public static string FormatTwoDecimalPlace(float value) => string.Format("{0:0.00}", (object) value);

  public static string FormatOneDecimalPlace(float value) => string.Format("{0:0.0}", (object) value);

  public static string FormatWholeNumber(float value) => string.Format("{0:0}", (object) value);

  public static bool IsInputCharacterValid(char _char, bool isPath = false) => !Util.defaultInvalidUserInputChars.Contains(_char) && (isPath || !Util.additionalInvalidUserInputChars.Contains(_char));

  public static void ScrubInputField(TMP_InputField inputField, bool isPath = false)
  {
    for (int index = inputField.text.Length - 1; index >= 0; --index)
    {
      if (index < inputField.text.Length && !Util.IsInputCharacterValid(inputField.text[index], isPath))
        inputField.text = inputField.text.Remove(index, 1);
    }
  }

  public static string StripTextFormatting(string original) => Regex.Replace(original, "<[^>]*>([^<]*)<[^>]*>", "$1");

  public static void Reset(Transform transform)
  {
    transform.SetLocalPosition(Vector3.zero);
    transform.localRotation = Quaternion.identity;
    transform.localScale = Vector3.one;
  }

  public static float GaussianRandom(float mu = 0.0f, float sigma = 1f)
  {
    double num1 = Util.random.NextDouble();
    double num2 = Util.random.NextDouble();
    double num3 = (double) Mathf.Sqrt(-2f * Mathf.Log((float) num1)) * (double) Mathf.Sin(6.283185f * (float) num2);
    return mu + sigma * (float) num3;
  }

  public static void Shuffle<T>(this IList<T> list) => list.ShuffleSeeded<T>(Util.random);

  public static Bounds GetBounds(GameObject go)
  {
    Bounds bounds = new Bounds();
    bool first = true;
    Util.GetBounds(go, ref bounds, ref first);
    return bounds;
  }

  private static void GetBounds(GameObject go, ref Bounds bounds, ref bool first)
  {
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    MeshRenderer component = go.GetComponent<MeshRenderer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (first)
      {
        bounds = component.bounds;
        first = false;
      }
      else
        bounds.Encapsulate(component.bounds);
    }
    for (int index = 0; index < go.transform.childCount; ++index)
      Util.GetBounds(go.transform.GetChild(index).gameObject, ref bounds, ref first);
  }

  public static bool IsOnLeftSideOfScreen(Vector3 position) => (double) position.x < (double) Screen.width;

  public static void Write(this BinaryWriter writer, Vector2 v)
  {
    writer.WriteSingleFast(v.x);
    writer.WriteSingleFast(v.y);
  }

  public static void Write(this BinaryWriter writer, Vector3 v)
  {
    writer.WriteSingleFast(v.x);
    writer.WriteSingleFast(v.y);
    writer.WriteSingleFast(v.z);
  }

  public static Vector2 ReadVector2(this BinaryReader reader) => new Vector2()
  {
    x = reader.ReadSingle(),
    y = reader.ReadSingle()
  };

  public static Vector3 ReadVector3(this BinaryReader reader) => new Vector3()
  {
    x = reader.ReadSingle(),
    y = reader.ReadSingle(),
    z = reader.ReadSingle()
  };

  public static void Write(this BinaryWriter writer, Quaternion q)
  {
    writer.WriteSingleFast(q.x);
    writer.WriteSingleFast(q.y);
    writer.WriteSingleFast(q.z);
    writer.WriteSingleFast(q.w);
  }

  public static Quaternion ReadQuaternion(this BinaryReader reader) => new Quaternion()
  {
    x = reader.ReadSingle(),
    y = reader.ReadSingle(),
    z = reader.ReadSingle(),
    w = reader.ReadSingle()
  };

  public static Color ColorFromHex(string hex)
  {
    int int32 = Convert.ToInt32(hex, 16);
    float r = 1f;
    float g = 1f;
    float b = 1f;
    float a = 1f;
    if (hex.Length == 6)
    {
      r = (float) (int32 >> 16 & (int) byte.MaxValue) / (float) byte.MaxValue;
      g = (float) (int32 >> 8 & (int) byte.MaxValue) / (float) byte.MaxValue;
      b = (float) (int32 & (int) byte.MaxValue) / (float) byte.MaxValue;
    }
    else if (hex.Length == 8)
    {
      r = (float) (int32 >> 24 & (int) byte.MaxValue) / (float) byte.MaxValue;
      g = (float) (int32 >> 16 & (int) byte.MaxValue) / (float) byte.MaxValue;
      b = (float) (int32 >> 8 & (int) byte.MaxValue) / (float) byte.MaxValue;
      a = (float) (int32 & (int) byte.MaxValue) / (float) byte.MaxValue;
    }
    return new Color(r, g, b, a);
  }

  public static string ToHexString(this Color c) => string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", (object) (int) ((double) c.r * (double) byte.MaxValue), (object) (int) ((double) c.g * (double) byte.MaxValue), (object) (int) ((double) c.b * (double) byte.MaxValue), (object) (int) ((double) c.a * (double) byte.MaxValue));

  public static void Signal(this System.Action action)
  {
    if (action == null)
      return;
    action();
  }

  public static void Signal<T>(this System.Action<T> action, T parameter)
  {
    if (action == null)
      return;
    action(parameter);
  }

  public static RectTransform rectTransform(this GameObject go) => go.GetComponent<RectTransform>();

  public static RectTransform rectTransform(this Component cmp) => cmp.GetComponent<RectTransform>();

  public static T[] Append<T>(this T[] array, T item)
  {
    T[] objArray = new T[array.Length + 1];
    for (int index = 0; index < array.Length; ++index)
      objArray[index] = array[index];
    objArray[array.Length] = item;
    return objArray;
  }

  public static string GetKleiRootPath() => Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Klei") : Util.defaultRootFolder;

  public static string GetTitleFolderName() => "OxygenNotIncluded";

  public static string GetRetiredColoniesFolderName() => "RetiredColonies";

  public static string RootFolder() => Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? Path.Combine(Util.GetKleiRootPath(), Util.GetTitleFolderName()) : Util.GetKleiRootPath();

  public static string LogsFolder() => Path.GetDirectoryName(Application.consoleLogPath);

  public static string CacheFolder() => Path.Combine(Util.defaultRootFolder, "cache");

  public static Transform FindTransformRecursive(Transform node, string name)
  {
    if (node.name == name)
      return node;
    for (int index = 0; index < node.childCount; ++index)
    {
      Transform transformRecursive = Util.FindTransformRecursive(node.GetChild(index), name);
      if ((UnityEngine.Object) transformRecursive != (UnityEngine.Object) null)
        return transformRecursive;
    }
    return (Transform) null;
  }

  public static Vector3 ReadVector3(this IReader reader) => new Vector3()
  {
    x = reader.ReadSingle(),
    y = reader.ReadSingle(),
    z = reader.ReadSingle()
  };

  public static Quaternion ReadQuaternion(this IReader reader) => new Quaternion()
  {
    x = reader.ReadSingle(),
    y = reader.ReadSingle(),
    z = reader.ReadSingle(),
    w = reader.ReadSingle()
  };

  public static T GetRandom<T>(this T[] tArray) => tArray[UnityEngine.Random.Range(0, tArray.Length)];

  public static T GetRandom<T>(this List<T> tList) => tList[UnityEngine.Random.Range(0, tList.Count)];

  public static float RandomVariance(float center, float plusminus) => center + UnityEngine.Random.Range(-plusminus, plusminus);

  public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrEmpty(str) || str == " ";

  public static void ApplyInvariantCultureToThread(Thread thread)
  {
    if (Application.platform == RuntimePlatform.WindowsEditor)
      return;
    thread.CurrentCulture = CultureInfo.InvariantCulture;
  }
}
