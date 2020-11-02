// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Utilities.GUIScaleUtility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NodeEditorFramework.Utilities
{
  public static class GUIScaleUtility
  {
    private static bool compabilityMode;
    private static bool initiated;
    private static FieldInfo currentGUILayoutCache;
    private static FieldInfo currentTopLevelGroup;
    private static Func<Rect> GetTopRectDelegate;
    private static Func<Rect> topmostRectDelegate;
    private static List<List<Rect>> rectStackGroups;
    private static List<Matrix4x4> GUIMatrices;
    private static List<bool> adjustedGUILayout;

    public static Rect getTopRect => GUIScaleUtility.GetTopRectDelegate();

    public static Rect getTopRectScreenSpace => GUIScaleUtility.topmostRectDelegate();

    public static List<Rect> currentRectStack { get; private set; }

    public static void CheckInit()
    {
      if (GUIScaleUtility.initiated)
        return;
      GUIScaleUtility.Init();
    }

    public static void Init()
    {
      System.Type type = Assembly.GetAssembly(typeof (GUI)).GetType("UnityEngine.GUIClip", true);
      PropertyInfo property = type.GetProperty("topmostRect", BindingFlags.Static | BindingFlags.Public);
      MethodInfo method1 = type.GetMethod("GetTopRect", BindingFlags.Static | BindingFlags.NonPublic);
      MethodInfo method2 = type.GetMethod("Clip", BindingFlags.Static | BindingFlags.Public, System.Type.DefaultBinder, new System.Type[1]
      {
        typeof (Rect)
      }, new ParameterModifier[0]);
      if (type == (System.Type) null || property == (PropertyInfo) null || (method1 == (MethodInfo) null || method2 == (MethodInfo) null))
      {
        Debug.LogWarning((object) "GUIScaleUtility cannot run on this system! Compability mode enabled. For you that means you're not able to use the Node Editor inside more than one group:( Please PM me (Seneral @UnityForums) so I can figure out what causes this! Thanks!");
        Debug.LogWarning((object) ((type == (System.Type) null ? "GUIClipType is Null, " : "") + (property == (PropertyInfo) null ? "topmostRect is Null, " : "") + (method1 == (MethodInfo) null ? "GetTopRect is Null, " : "") + (method2 == (MethodInfo) null ? "ClipRect is Null, " : "")));
        GUIScaleUtility.compabilityMode = true;
        GUIScaleUtility.initiated = true;
      }
      else
      {
        GUIScaleUtility.GetTopRectDelegate = (Func<Rect>) Delegate.CreateDelegate(typeof (Func<Rect>), method1);
        GUIScaleUtility.topmostRectDelegate = (Func<Rect>) Delegate.CreateDelegate(typeof (Func<Rect>), property.GetGetMethod());
        if (GUIScaleUtility.GetTopRectDelegate == null || GUIScaleUtility.topmostRectDelegate == null)
        {
          Debug.LogWarning((object) "GUIScaleUtility cannot run on this system! Compability mode enabled. For you that means you're not able to use the Node Editor inside more than one group:( Please PM me (Seneral @UnityForums) so I can figure out what causes this! Thanks!");
          Debug.LogWarning((object) ((type == (System.Type) null ? "GUIClipType is Null, " : "") + (property == (PropertyInfo) null ? "topmostRect is Null, " : "") + (method1 == (MethodInfo) null ? "GetTopRect is Null, " : "") + (method2 == (MethodInfo) null ? "ClipRect is Null, " : "")));
          GUIScaleUtility.compabilityMode = true;
          GUIScaleUtility.initiated = true;
        }
        else
        {
          GUIScaleUtility.currentRectStack = new List<Rect>();
          GUIScaleUtility.rectStackGroups = new List<List<Rect>>();
          GUIScaleUtility.GUIMatrices = new List<Matrix4x4>();
          GUIScaleUtility.adjustedGUILayout = new List<bool>();
          GUIScaleUtility.initiated = true;
        }
      }
    }

    public static Vector2 getCurrentScale => new Vector2(1f / GUI.matrix.GetColumn(0).magnitude, 1f / GUI.matrix.GetColumn(1).magnitude);

    public static Vector2 BeginScale(
      ref Rect rect,
      Vector2 zoomPivot,
      float zoom,
      bool adjustGUILayout)
    {
      Rect rect1;
      if (GUIScaleUtility.compabilityMode)
      {
        GUI.EndGroup();
        rect1 = rect;
      }
      else
      {
        GUIScaleUtility.BeginNoClip();
        rect1 = GUIScaleUtility.GUIToScaledSpace(rect);
      }
      rect = GUIScaleUtility.Scale(rect1, rect1.position + zoomPivot, new Vector2(zoom, zoom));
      GUI.BeginGroup(rect);
      rect.position = Vector2.zero;
      Vector2 pivotPoint = rect.center - rect1.size / 2f + zoomPivot;
      GUIScaleUtility.adjustedGUILayout.Add(adjustGUILayout);
      if (adjustGUILayout)
      {
        GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.Space(rect.center.x - rect1.size.x + zoomPivot.x);
        GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.Space(rect.center.y - rect1.size.y + zoomPivot.y);
      }
      GUIScaleUtility.GUIMatrices.Add(GUI.matrix);
      GUIUtility.ScaleAroundPivot(new Vector2(1f / zoom, 1f / zoom), pivotPoint);
      return pivotPoint;
    }

    public static void EndScale()
    {
      if (GUIScaleUtility.GUIMatrices.Count == 0 || GUIScaleUtility.adjustedGUILayout.Count == 0)
        throw new UnityException("GUIScaleUtility: You are ending more scale regions than you are beginning!");
      GUI.matrix = GUIScaleUtility.GUIMatrices[GUIScaleUtility.GUIMatrices.Count - 1];
      GUIScaleUtility.GUIMatrices.RemoveAt(GUIScaleUtility.GUIMatrices.Count - 1);
      if (GUIScaleUtility.adjustedGUILayout[GUIScaleUtility.adjustedGUILayout.Count - 1])
      {
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
      }
      GUIScaleUtility.adjustedGUILayout.RemoveAt(GUIScaleUtility.adjustedGUILayout.Count - 1);
      GUI.EndGroup();
      if (GUIScaleUtility.compabilityMode)
      {
        if (!Application.isPlaying)
          GUI.BeginClip(new Rect(0.0f, 23f, (float) Screen.width, (float) (Screen.height - 23)));
        else
          GUI.BeginClip(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height));
      }
      else
        GUIScaleUtility.RestoreClips();
    }

    public static void BeginNoClip()
    {
      List<Rect> rectList = new List<Rect>();
      for (Rect getTopRect = GUIScaleUtility.getTopRect; getTopRect != new Rect(-10000f, -10000f, 40000f, 40000f); getTopRect = GUIScaleUtility.getTopRect)
      {
        rectList.Add(getTopRect);
        GUI.EndClip();
      }
      rectList.Reverse();
      GUIScaleUtility.rectStackGroups.Add(rectList);
      GUIScaleUtility.currentRectStack.AddRange((IEnumerable<Rect>) rectList);
    }

    public static void MoveClipsUp(int count)
    {
      List<Rect> rectList = new List<Rect>();
      for (Rect getTopRect = GUIScaleUtility.getTopRect; getTopRect != new Rect(-10000f, -10000f, 40000f, 40000f) && count > 0; --count)
      {
        rectList.Add(getTopRect);
        GUI.EndClip();
        getTopRect = GUIScaleUtility.getTopRect;
      }
      rectList.Reverse();
      GUIScaleUtility.rectStackGroups.Add(rectList);
      GUIScaleUtility.currentRectStack.AddRange((IEnumerable<Rect>) rectList);
    }

    public static void RestoreClips()
    {
      if (GUIScaleUtility.rectStackGroups.Count == 0)
      {
        Debug.LogError((object) "GUIClipHierarchy: BeginNoClip/MoveClipsUp - RestoreClips count not balanced!");
      }
      else
      {
        List<Rect> rectStackGroup = GUIScaleUtility.rectStackGroups[GUIScaleUtility.rectStackGroups.Count - 1];
        for (int index = 0; index < rectStackGroup.Count; ++index)
        {
          GUI.BeginClip(rectStackGroup[index]);
          GUIScaleUtility.currentRectStack.RemoveAt(GUIScaleUtility.currentRectStack.Count - 1);
        }
        GUIScaleUtility.rectStackGroups.RemoveAt(GUIScaleUtility.rectStackGroups.Count - 1);
      }
    }

    public static void BeginNewLayout()
    {
      if (GUIScaleUtility.compabilityMode)
        return;
      Rect getTopRect = GUIScaleUtility.getTopRect;
      if (getTopRect != new Rect(-10000f, -10000f, 40000f, 40000f))
        GUILayout.BeginArea(new Rect(0.0f, 0.0f, getTopRect.width, getTopRect.height));
      else
        GUILayout.BeginArea(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height));
    }

    public static void EndNewLayout()
    {
      if (GUIScaleUtility.compabilityMode)
        return;
      GUILayout.EndArea();
    }

    public static void BeginIgnoreMatrix()
    {
      GUIScaleUtility.GUIMatrices.Add(GUI.matrix);
      GUI.matrix = Matrix4x4.identity;
    }

    public static void EndIgnoreMatrix()
    {
      GUI.matrix = GUIScaleUtility.GUIMatrices.Count != 0 ? GUIScaleUtility.GUIMatrices[GUIScaleUtility.GUIMatrices.Count - 1] : throw new UnityException("GUIScaleutility: You are ending more ignoreMatrices than you are beginning!");
      GUIScaleUtility.GUIMatrices.RemoveAt(GUIScaleUtility.GUIMatrices.Count - 1);
    }

    public static Vector2 Scale(Vector2 pos, Vector2 pivot, Vector2 scale) => Vector2.Scale(pos - pivot, scale) + pivot;

    public static Rect Scale(Rect rect, Vector2 pivot, Vector2 scale)
    {
      rect.position = Vector2.Scale(rect.position - pivot, scale) + pivot;
      rect.size = Vector2.Scale(rect.size, scale);
      return rect;
    }

    public static Vector2 ScaledToGUISpace(Vector2 scaledPosition)
    {
      if (GUIScaleUtility.rectStackGroups == null || GUIScaleUtility.rectStackGroups.Count == 0)
        return scaledPosition;
      List<Rect> rectStackGroup = GUIScaleUtility.rectStackGroups[GUIScaleUtility.rectStackGroups.Count - 1];
      for (int index = 0; index < rectStackGroup.Count; ++index)
        scaledPosition -= rectStackGroup[index].position;
      return scaledPosition;
    }

    public static Rect ScaledToGUISpace(Rect scaledRect)
    {
      if (GUIScaleUtility.rectStackGroups == null || GUIScaleUtility.rectStackGroups.Count == 0)
        return scaledRect;
      scaledRect.position = GUIScaleUtility.ScaledToGUISpace(scaledRect.position);
      return scaledRect;
    }

    public static Vector2 GUIToScaledSpace(Vector2 guiPosition)
    {
      if (GUIScaleUtility.rectStackGroups == null || GUIScaleUtility.rectStackGroups.Count == 0)
        return guiPosition;
      List<Rect> rectStackGroup = GUIScaleUtility.rectStackGroups[GUIScaleUtility.rectStackGroups.Count - 1];
      for (int index = 0; index < rectStackGroup.Count; ++index)
        guiPosition += rectStackGroup[index].position;
      return guiPosition;
    }

    public static Rect GUIToScaledSpace(Rect guiRect)
    {
      if (GUIScaleUtility.rectStackGroups == null || GUIScaleUtility.rectStackGroups.Count == 0)
        return guiRect;
      guiRect.position = GUIScaleUtility.GUIToScaledSpace(guiRect.position);
      return guiRect;
    }

    public static Vector2 GUIToScreenSpace(Vector2 guiPosition) => guiPosition + GUIScaleUtility.getTopRectScreenSpace.position;

    public static Rect GUIToScreenSpace(Rect guiRect)
    {
      guiRect.position += GUIScaleUtility.getTopRectScreenSpace.position;
      return guiRect;
    }
  }
}
