// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Utilities.RTEditorGUI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace NodeEditorFramework.Utilities
{
  public static class RTEditorGUI
  {
    public static float labelWidth = 150f;
    public static float fieldWidth = 50f;
    public static float indent = 0.0f;
    private static GUIStyle seperator;
    private static Stack<bool> changeStack = new Stack<bool>();
    private static int activeFloatField = -1;
    private static float activeFloatFieldLastValue = 0.0f;
    private static string activeFloatFieldString = "";
    private static Material texVizMat;
    private static Material lineMaterial;
    private static Texture2D lineTexture;

    private static float textFieldHeight => GUI.skin.textField.CalcHeight(new GUIContent("i"), 10f);

    public static Rect PrefixLabel(Rect totalPos, GUIContent label, GUIStyle style)
    {
      if (label == GUIContent.none)
        return totalPos;
      GUI.Label(new Rect(totalPos.x + RTEditorGUI.indent, totalPos.y, Mathf.Min(RTEditorGUI.getLabelWidth() - RTEditorGUI.indent, totalPos.width / 2f), totalPos.height), label, style);
      return new Rect(totalPos.x + RTEditorGUI.getLabelWidth(), totalPos.y, totalPos.width - RTEditorGUI.getLabelWidth(), totalPos.height);
    }

    public static Rect PrefixLabel(
      Rect totalPos,
      float percentage,
      GUIContent label,
      GUIStyle style)
    {
      if (label == GUIContent.none)
        return totalPos;
      GUI.Label(new Rect(totalPos.x + RTEditorGUI.indent, totalPos.y, totalPos.width * percentage, totalPos.height), label, style);
      return new Rect(totalPos.x + totalPos.width * percentage, totalPos.y, totalPos.width * (1f - percentage), totalPos.height);
    }

    private static Rect IndentedRect(Rect source) => new Rect(source.x + RTEditorGUI.indent, source.y, source.width - RTEditorGUI.indent, source.height);

    private static float getLabelWidth() => (double) RTEditorGUI.labelWidth == 0.0 ? 150f : RTEditorGUI.labelWidth;

    private static float getFieldWidth() => (double) RTEditorGUI.fieldWidth == 0.0 ? 50f : RTEditorGUI.fieldWidth;

    private static Rect GetFieldRect(
      GUIContent label,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      float minWidth = 0.0f;
      float maxWidth = 0.0f;
      if (label != GUIContent.none)
        style.CalcMinMaxWidth(label, out minWidth, out maxWidth);
      return GUILayoutUtility.GetRect((float) ((double) RTEditorGUI.getFieldWidth() + (double) minWidth + 5.0), (float) ((double) RTEditorGUI.getFieldWidth() + (double) maxWidth + 5.0), RTEditorGUI.textFieldHeight, RTEditorGUI.textFieldHeight, options);
    }

    private static Rect GetSliderRect(
      GUIContent label,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      float minWidth = 0.0f;
      float maxWidth = 0.0f;
      if (label != GUIContent.none)
        style.CalcMinMaxWidth(label, out minWidth, out maxWidth);
      return GUILayoutUtility.GetRect((float) ((double) RTEditorGUI.getFieldWidth() + (double) minWidth + 5.0), (float) ((double) RTEditorGUI.getFieldWidth() + (double) maxWidth + 5.0 + 100.0), RTEditorGUI.textFieldHeight, RTEditorGUI.textFieldHeight, options);
    }

    private static Rect GetSliderRect(Rect sliderRect) => new Rect(sliderRect.x, sliderRect.y, (float) ((double) sliderRect.width - (double) RTEditorGUI.getFieldWidth() - 5.0), sliderRect.height);

    private static Rect GetSliderFieldRect(Rect sliderRect) => new Rect(sliderRect.x + sliderRect.width - RTEditorGUI.getFieldWidth(), sliderRect.y, RTEditorGUI.getFieldWidth(), sliderRect.height);

    public static void Space() => RTEditorGUI.Space(6f);

    public static void Space(float pixels) => GUILayoutUtility.GetRect(pixels, pixels);

    public static void Seperator()
    {
      RTEditorGUI.setupSeperator();
      GUILayout.Box(GUIContent.none, RTEditorGUI.seperator, GUILayout.Height(1f));
    }

    public static void Seperator(Rect rect)
    {
      RTEditorGUI.setupSeperator();
      GUI.Box(new Rect(rect.x, rect.y, rect.width, 1f), GUIContent.none, RTEditorGUI.seperator);
    }

    private static void setupSeperator()
    {
      if (RTEditorGUI.seperator != null)
        return;
      RTEditorGUI.seperator = new GUIStyle();
      RTEditorGUI.seperator.normal.background = RTEditorGUI.ColorToTex(1, new Color(0.6f, 0.6f, 0.6f));
      RTEditorGUI.seperator.stretchWidth = true;
      RTEditorGUI.seperator.margin = new RectOffset(0, 0, 7, 7);
    }

    public static void BeginChangeCheck()
    {
      RTEditorGUI.changeStack.Push(GUI.changed);
      GUI.changed = false;
    }

    public static bool EndChangeCheck()
    {
      bool changed = GUI.changed;
      if (RTEditorGUI.changeStack.Count > 0)
      {
        GUI.changed = RTEditorGUI.changeStack.Pop();
        if (changed && RTEditorGUI.changeStack.Count > 0 && !RTEditorGUI.changeStack.Peek())
        {
          RTEditorGUI.changeStack.Pop();
          RTEditorGUI.changeStack.Push(changed);
        }
      }
      else
        Debug.LogWarning((object) "Requesting more EndChangeChecks than issuing BeginChangeChecks!");
      return changed;
    }

    public static bool Foldout(bool foldout, string content, params GUILayoutOption[] options) => RTEditorGUI.Foldout(foldout, new GUIContent(content), options);

    public static bool Foldout(
      bool foldout,
      string content,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      return RTEditorGUI.Foldout(foldout, new GUIContent(content), style, options);
    }

    public static bool Foldout(bool foldout, GUIContent content, params GUILayoutOption[] options) => RTEditorGUI.Foldout(foldout, content, GUI.skin.toggle, options);

    public static bool Foldout(
      bool foldout,
      GUIContent content,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      return GUILayout.Toggle(foldout, content, style, options);
    }

    public static bool Toggle(bool toggle, string content, params GUILayoutOption[] options) => RTEditorGUI.Toggle(toggle, new GUIContent(content), options);

    public static bool Toggle(
      bool toggle,
      string content,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      return RTEditorGUI.Toggle(toggle, new GUIContent(content), style, options);
    }

    public static bool Toggle(bool toggle, GUIContent content, params GUILayoutOption[] options) => RTEditorGUI.Toggle(toggle, content, GUI.skin.toggle, options);

    public static bool Toggle(
      bool toggle,
      GUIContent content,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      return GUILayout.Toggle(toggle, content, style, options);
    }

    public static string TextField(
      GUIContent label,
      string text,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      if (style == null)
        style = GUI.skin.textField;
      text = GUI.TextField(RTEditorGUI.PrefixLabel(RTEditorGUI.GetFieldRect(label, style, options), 0.5f, label, style), text);
      return text;
    }

    public static int OptionSlider(
      GUIContent label,
      int selected,
      string[] selectableOptions,
      params GUILayoutOption[] options)
    {
      return RTEditorGUI.OptionSlider(label, selected, selectableOptions, GUI.skin.label, options);
    }

    public static int OptionSlider(
      GUIContent label,
      int selected,
      string[] selectableOptions,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      if (style == null)
        style = GUI.skin.textField;
      Rect sliderRect = RTEditorGUI.PrefixLabel(RTEditorGUI.GetSliderRect(label, style, options), 0.5f, label, style);
      selected = Mathf.RoundToInt(GUI.HorizontalSlider(RTEditorGUI.GetSliderRect(sliderRect), (float) selected, 0.0f, (float) (selectableOptions.Length - 1)));
      GUI.Label(RTEditorGUI.GetSliderFieldRect(sliderRect), selectableOptions[selected]);
      return selected;
    }

    public static int MathPowerSlider(
      GUIContent label,
      int baseValue,
      int value,
      int minPow,
      int maxPow,
      params GUILayoutOption[] options)
    {
      int power = (int) Math.Floor(Math.Log((double) value) / Math.Log((double) baseValue));
      int num = RTEditorGUI.MathPowerSliderRaw(label, baseValue, power, minPow, maxPow, options);
      return (int) Math.Pow((double) baseValue, (double) num);
    }

    public static int MathPowerSliderRaw(
      GUIContent label,
      int baseValue,
      int power,
      int minPow,
      int maxPow,
      params GUILayoutOption[] options)
    {
      Rect sliderRect = RTEditorGUI.PrefixLabel(RTEditorGUI.GetSliderRect(label, GUI.skin.label, options), 0.5f, label, GUI.skin.label);
      power = Mathf.RoundToInt(GUI.HorizontalSlider(RTEditorGUI.GetSliderRect(sliderRect), (float) power, (float) minPow, (float) maxPow));
      GUI.Label(RTEditorGUI.GetSliderFieldRect(sliderRect), Mathf.Pow((float) baseValue, (float) power).ToString());
      return power;
    }

    public static int IntSlider(
      string label,
      int value,
      int minValue,
      int maxValue,
      params GUILayoutOption[] options)
    {
      return (int) RTEditorGUI.Slider(new GUIContent(label), (float) value, (float) minValue, (float) maxValue, options);
    }

    public static int IntSlider(
      GUIContent label,
      int value,
      int minValue,
      int maxValue,
      params GUILayoutOption[] options)
    {
      return (int) RTEditorGUI.Slider(label, (float) value, (float) minValue, (float) maxValue, options);
    }

    public static int IntSlider(
      int value,
      int minValue,
      int maxValue,
      params GUILayoutOption[] options)
    {
      return (int) RTEditorGUI.Slider(GUIContent.none, (float) value, (float) minValue, (float) maxValue, options);
    }

    public static int IntField(string label, int value, params GUILayoutOption[] options) => (int) RTEditorGUI.FloatField(new GUIContent(label), (float) value, options);

    public static int IntField(GUIContent label, int value, params GUILayoutOption[] options) => (int) RTEditorGUI.FloatField(label, (float) value, options);

    public static int IntField(int value, params GUILayoutOption[] options) => (int) RTEditorGUI.FloatField((float) value, options);

    public static float Slider(
      float value,
      float minValue,
      float maxValue,
      params GUILayoutOption[] options)
    {
      return RTEditorGUI.Slider(GUIContent.none, value, minValue, maxValue, options);
    }

    public static float Slider(
      string label,
      float value,
      float minValue,
      float maxValue,
      params GUILayoutOption[] options)
    {
      return RTEditorGUI.Slider(new GUIContent(label), value, minValue, maxValue, options);
    }

    public static float Slider(
      GUIContent label,
      float value,
      float minValue,
      float maxValue,
      params GUILayoutOption[] options)
    {
      Rect sliderRect = RTEditorGUI.PrefixLabel(RTEditorGUI.GetSliderRect(label, GUI.skin.label, options), 0.5f, label, GUI.skin.label);
      value = GUI.HorizontalSlider(RTEditorGUI.GetSliderRect(sliderRect), value, minValue, maxValue);
      value = Mathf.Min(maxValue, Mathf.Max(minValue, RTEditorGUI.FloatField(RTEditorGUI.GetSliderFieldRect(sliderRect), value, GUILayout.Width(60f))));
      return value;
    }

    public static float FloatField(
      string label,
      float value,
      params GUILayoutOption[] fieldOptions)
    {
      return RTEditorGUI.FloatField(new GUIContent(label), value, fieldOptions);
    }

    public static float FloatField(GUIContent label, float value, params GUILayoutOption[] options) => RTEditorGUI.FloatField(RTEditorGUI.PrefixLabel(RTEditorGUI.GetFieldRect(label, GUI.skin.label, options), 0.5f, label, GUI.skin.label), value, options);

    public static float FloatField(float value, params GUILayoutOption[] options) => RTEditorGUI.FloatField(RTEditorGUI.GetFieldRect(GUIContent.none, (GUIStyle) null, options), value, options);

    public static float FloatField(Rect pos, float value, params GUILayoutOption[] options)
    {
      int num = GUIUtility.GetControlID(nameof (FloatField).GetHashCode(), FocusType.Keyboard, pos) + 1;
      if (num == 0)
        return value;
      bool flag1 = RTEditorGUI.activeFloatField == num;
      bool flag2 = num == GUIUtility.keyboardControl;
      if (flag2 & flag1 && (double) RTEditorGUI.activeFloatFieldLastValue != (double) value)
      {
        RTEditorGUI.activeFloatFieldLastValue = value;
        RTEditorGUI.activeFloatFieldString = value.ToString();
      }
      string text = flag1 ? RTEditorGUI.activeFloatFieldString : value.ToString();
      string str = GUI.TextField(pos, text);
      if (flag1)
        RTEditorGUI.activeFloatFieldString = str;
      bool flag3 = true;
      if (str == "")
        value = RTEditorGUI.activeFloatFieldLastValue = 0.0f;
      else if (str != value.ToString())
      {
        float result;
        flag3 = float.TryParse(str, out result);
        if (flag3)
          value = RTEditorGUI.activeFloatFieldLastValue = result;
      }
      if (flag2 && !flag1)
      {
        RTEditorGUI.activeFloatField = num;
        RTEditorGUI.activeFloatFieldString = str;
        RTEditorGUI.activeFloatFieldLastValue = value;
      }
      else if (!flag2 & flag1)
      {
        RTEditorGUI.activeFloatField = -1;
        if (!flag3)
          value = str.ForceParse();
      }
      return value;
    }

    public static float ForceParse(this string str)
    {
      float result;
      if (float.TryParse(str, out result))
        return result;
      bool flag = false;
      List<char> charList = new List<char>((IEnumerable<char>) str);
      for (int index = 0; index < charList.Count; ++index)
      {
        if (CharUnicodeInfo.GetUnicodeCategory(str[index]) != UnicodeCategory.DecimalDigitNumber)
        {
          charList.RemoveRange(index, charList.Count - index);
          break;
        }
        if (str[index] == '.')
        {
          if (flag)
          {
            charList.RemoveRange(index, charList.Count - index);
            break;
          }
          flag = true;
        }
      }
      if (charList.Count == 0)
        return 0.0f;
      str = new string(charList.ToArray());
      if (!float.TryParse(str, out result))
        Debug.LogError((object) ("Could not parse " + str));
      return result;
    }

    public static T ObjectField<T>(T obj, bool allowSceneObjects) where T : UnityEngine.Object => RTEditorGUI.ObjectField<T>(GUIContent.none, obj, allowSceneObjects, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());

    public static T ObjectField<T>(string label, T obj, bool allowSceneObjects) where T : UnityEngine.Object => RTEditorGUI.ObjectField<T>(new GUIContent(label), obj, allowSceneObjects, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());

    public static T ObjectField<T>(
      GUIContent label,
      T obj,
      bool allowSceneObjects,
      params GUILayoutOption[] options)
      where T : UnityEngine.Object
    {
      bool flag;
      if (obj.GetType() == typeof (Texture2D))
      {
        GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.Label(label, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        flag = GUILayout.Button((Texture) ((object) obj as Texture2D), GUILayout.MaxWidth(64f), GUILayout.MaxHeight(64f));
        GUILayout.EndHorizontal();
      }
      else
      {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        flag = GUILayout.Button(label, style, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      }
      int num = flag ? 1 : 0;
      return obj;
    }

    public static Enum EnumPopup(Enum selected) => RTEditorGUI.EnumPopup(GUIContent.none, selected);

    public static Enum EnumPopup(string label, Enum selected) => RTEditorGUI.EnumPopup(new GUIContent(label), selected);

    public static Enum EnumPopup(GUIContent label, Enum selected)
    {
      GUIContent guiContent = label;
      guiContent.text = guiContent.text + ": " + selected.ToString();
      GUILayout.Label(label, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      return selected;
    }

    public static int Popup(GUIContent label, int selected, string[] displayedOptions)
    {
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUIContent guiContent = label;
      guiContent.text = guiContent.text + ": " + selected.ToString();
      GUILayout.Label(label, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.EndHorizontal();
      return selected;
    }

    public static int Popup(string label, int selected, string[] displayedOptions)
    {
      GUILayout.Label(label + ": " + selected.ToString(), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      return selected;
    }

    public static int Popup(int selected, string[] displayedOptions) => RTEditorGUI.Popup("", selected, displayedOptions);

    public static void DrawTexture(
      Texture texture,
      int texSize,
      GUIStyle style,
      params GUILayoutOption[] options)
    {
      RTEditorGUI.DrawTexture(texture, texSize, style, 1, 2, 3, 4, options);
    }

    public static void DrawTexture(
      Texture texture,
      int texSize,
      GUIStyle style,
      int shuffleRed,
      int shuffleGreen,
      int shuffleBlue,
      int shuffleAlpha,
      params GUILayoutOption[] options)
    {
      if ((UnityEngine.Object) RTEditorGUI.texVizMat == (UnityEngine.Object) null)
        RTEditorGUI.texVizMat = new Material(Shader.Find("Hidden/GUITextureClip_ChannelControl"));
      RTEditorGUI.texVizMat.SetInt(nameof (shuffleRed), shuffleRed);
      RTEditorGUI.texVizMat.SetInt(nameof (shuffleGreen), shuffleGreen);
      RTEditorGUI.texVizMat.SetInt(nameof (shuffleBlue), shuffleBlue);
      RTEditorGUI.texVizMat.SetInt(nameof (shuffleAlpha), shuffleAlpha);
      if (options == null || options.Length == 0)
        options = new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        };
      Rect screenRect = style == null ? GUILayoutUtility.GetRect((float) texSize, (float) texSize, options) : GUILayoutUtility.GetRect((float) texSize, (float) texSize, style, options);
      if (UnityEngine.Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawTexture(screenRect, texture, RTEditorGUI.texVizMat);
    }

    private static void SetupLineMat(Texture tex, Color col)
    {
      if ((UnityEngine.Object) RTEditorGUI.lineMaterial == (UnityEngine.Object) null)
        RTEditorGUI.lineMaterial = new Material(Shader.Find("Hidden/LineShader"));
      if ((UnityEngine.Object) tex == (UnityEngine.Object) null)
        tex = (UnityEngine.Object) RTEditorGUI.lineTexture != (UnityEngine.Object) null ? (Texture) RTEditorGUI.lineTexture : (Texture) (RTEditorGUI.lineTexture = ResourceManager.LoadTexture("Textures/AALine.png"));
      RTEditorGUI.lineMaterial.SetTexture("_LineTexture", tex);
      RTEditorGUI.lineMaterial.SetColor("_LineColor", col);
      RTEditorGUI.lineMaterial.SetPass(0);
    }

    public static void DrawBezier(
      Vector2 startPos,
      Vector2 endPos,
      Vector2 startTan,
      Vector2 endTan,
      Color col,
      Texture2D tex,
      float width = 1f)
    {
      if (UnityEngine.Event.current.type != EventType.Repaint)
        return;
      int bezierSegmentCount = RTEditorGUI.CalculateBezierSegmentCount(startPos, endPos, startTan, endTan);
      RTEditorGUI.DrawBezier(startPos, endPos, startTan, endTan, col, tex, bezierSegmentCount, width);
    }

    public static void DrawBezier(
      Vector2 startPos,
      Vector2 endPos,
      Vector2 startTan,
      Vector2 endTan,
      Color col,
      Texture2D tex,
      int segmentCount,
      float width)
    {
      if (UnityEngine.Event.current.type != EventType.Repaint && UnityEngine.Event.current.type != EventType.KeyDown)
        return;
      Vector2[] points = new Vector2[segmentCount + 1];
      for (int index = 0; index <= segmentCount; ++index)
        points[index] = RTEditorGUI.GetBezierPoint((float) index / (float) segmentCount, startPos, endPos, startTan, endTan);
      RTEditorGUI.DrawPolygonLine(points, col, tex, width);
    }

    public static void DrawBezier(
      Rect clippingRect,
      Vector2 startPos,
      Vector2 endPos,
      Vector2 startTan,
      Vector2 endTan,
      Color col,
      Texture2D tex,
      int segmentCount,
      float width)
    {
      if (UnityEngine.Event.current.type != EventType.Repaint && UnityEngine.Event.current.type != EventType.KeyDown)
        return;
      Vector2[] points = new Vector2[segmentCount + 1];
      for (int index = 0; index <= segmentCount; ++index)
        points[index] = RTEditorGUI.GetBezierPoint((float) index / (float) segmentCount, startPos, endPos, startTan, endTan);
      RTEditorGUI.DrawPolygonLine(clippingRect, points, col, tex, width);
    }

    public static void DrawPolygonLine(Vector2[] points, Color col, Texture2D tex, float width = 1f) => RTEditorGUI.DrawPolygonLine(GUIScaleUtility.getTopRect, points, col, tex, width);

    public static void DrawPolygonLine(
      Rect clippingRect,
      Vector2[] points,
      Color col,
      Texture2D tex,
      float width = 1f)
    {
      if (UnityEngine.Event.current.type != EventType.Repaint && UnityEngine.Event.current.type != EventType.KeyDown || points.Length == 1)
        return;
      if (points.Length == 2)
        RTEditorGUI.DrawLine(points[0], points[1], col, tex, width);
      RTEditorGUI.SetupLineMat((Texture) tex, col);
      GL.Begin(5);
      GL.Color(Color.white);
      clippingRect.x = clippingRect.y = 0.0f;
      Vector2 p0 = points[0];
      for (int index = 1; index < points.Length; ++index)
      {
        Vector2 point = points[index];
        Vector2 vector2_1 = p0;
        Vector2 vector2_2 = point;
        bool clippedP0;
        bool clippedP1;
        if (RTEditorGUI.SegmentRectIntersection(clippingRect, ref p0, ref point, out clippedP0, out clippedP1))
        {
          Vector2 vector2_3 = index >= points.Length - 1 ? RTEditorGUI.CalculateLinePerpendicular(vector2_1, vector2_2) : RTEditorGUI.CalculatePointPerpendicular(vector2_1, vector2_2, points[index + 1]);
          if (clippedP0)
          {
            GL.End();
            GL.Begin(5);
            RTEditorGUI.DrawLineSegment(p0, vector2_3 * width / 2f);
          }
          if (index == 1)
            RTEditorGUI.DrawLineSegment(p0, RTEditorGUI.CalculateLinePerpendicular(p0, point) * width / 2f);
          RTEditorGUI.DrawLineSegment(point, vector2_3 * width / 2f);
        }
        else if (clippedP1)
        {
          GL.End();
          GL.Begin(5);
        }
        p0 = vector2_2;
      }
      GL.End();
    }

    private static int CalculateBezierSegmentCount(
      Vector2 startPos,
      Vector2 endPos,
      Vector2 startTan,
      Vector2 endTan)
    {
      return 4 + (int) ((double) (2f + Mathf.Pow((float) ((double) Vector2.Angle(startTan - startPos, endPos - startPos) * (double) Vector2.Angle(endTan - endPos, startPos - endPos) * ((double) endTan.magnitude + (double) startTan.magnitude)) / 400f, 0.125f)) * (double) Mathf.Pow(1f + (startPos - endPos).magnitude, 0.25f));
    }

    private static Vector2 CalculateLinePerpendicular(Vector2 startPos, Vector2 endPos) => new Vector2(endPos.y - startPos.y, startPos.x - endPos.x).normalized;

    private static Vector2 CalculatePointPerpendicular(
      Vector2 prevPos,
      Vector2 pointPos,
      Vector2 nextPos)
    {
      return RTEditorGUI.CalculateLinePerpendicular(pointPos, pointPos + (nextPos - prevPos));
    }

    private static Vector2 GetBezierPoint(
      float t,
      Vector2 startPos,
      Vector2 endPos,
      Vector2 startTan,
      Vector2 endTan)
    {
      float num1 = 1f - t;
      float num2 = num1 * t;
      return startPos * num1 * num1 * num1 + startTan * 3f * num1 * num2 + endTan * 3f * num2 * t + endPos * t * t * t;
    }

    private static void DrawLineSegment(Vector2 point, Vector2 perpendicular)
    {
      GL.TexCoord2(0.0f, 0.0f);
      GL.Vertex((Vector3) (point - perpendicular));
      GL.TexCoord2(0.0f, 1f);
      GL.Vertex((Vector3) (point + perpendicular));
    }

    public static void DrawLine(
      Vector2 startPos,
      Vector2 endPos,
      Color col,
      Texture2D tex,
      float width = 1f)
    {
      if (UnityEngine.Event.current.type != EventType.Repaint)
        return;
      RTEditorGUI.DrawLine(GUIScaleUtility.getTopRect, startPos, endPos, col, tex, width);
    }

    public static void DrawLine(
      Rect clippingRect,
      Vector2 startPos,
      Vector2 endPos,
      Color col,
      Texture2D tex,
      float width = 1f)
    {
      RTEditorGUI.SetupLineMat((Texture) tex, col);
      GL.Begin(5);
      GL.Color(Color.white);
      clippingRect.x = clippingRect.y = 0.0f;
      if (RTEditorGUI.SegmentRectIntersection(clippingRect, ref startPos, ref endPos))
      {
        Vector2 perpendicular = RTEditorGUI.CalculateLinePerpendicular(startPos, endPos) * width / 2f;
        RTEditorGUI.DrawLineSegment(startPos, perpendicular);
        RTEditorGUI.DrawLineSegment(endPos, perpendicular);
      }
      GL.End();
    }

    public static List<Vector2> GetLine(
      Rect clippingRect,
      Vector2 startPos,
      Vector2 endPos,
      float width = 1f,
      bool noClip = false)
    {
      List<Vector2> vector2List = new List<Vector2>();
      if (noClip || RTEditorGUI.SegmentRectIntersection(clippingRect, ref startPos, ref endPos))
      {
        Vector2 vector2 = RTEditorGUI.CalculateLinePerpendicular(startPos, endPos) * width / 2f;
        vector2List.Add(startPos - vector2);
        vector2List.Add(endPos + vector2);
      }
      return vector2List;
    }

    private static bool SegmentRectIntersection(Rect bounds, ref Vector2 p0, ref Vector2 p1) => RTEditorGUI.SegmentRectIntersection(bounds, ref p0, ref p1, out bool _, out bool _);

    private static bool SegmentRectIntersection(
      Rect bounds,
      ref Vector2 p0,
      ref Vector2 p1,
      out bool clippedP0,
      out bool clippedP1)
    {
      float t0 = 0.0f;
      float t1 = 1f;
      float p2 = p1.x - p0.x;
      float p3 = p1.y - p0.y;
      if (RTEditorGUI.ClipTest(-p2, p0.x - bounds.xMin, ref t0, ref t1) && RTEditorGUI.ClipTest(p2, bounds.xMax - p0.x, ref t0, ref t1) && (RTEditorGUI.ClipTest(-p3, p0.y - bounds.yMin, ref t0, ref t1) && RTEditorGUI.ClipTest(p3, bounds.yMax - p0.y, ref t0, ref t1)))
      {
        clippedP0 = (double) t0 > 0.0;
        clippedP1 = (double) t1 < 1.0;
        if (clippedP1)
        {
          p1.x = p0.x + t1 * p2;
          p1.y = p0.y + t1 * p3;
        }
        if (clippedP0)
        {
          p0.x += t0 * p2;
          p0.y += t0 * p3;
        }
        return true;
      }
      clippedP1 = clippedP0 = true;
      return false;
    }

    private static bool ClipTest(float p, float q, ref float t0, ref float t1)
    {
      float num = q / p;
      if ((double) p < 0.0)
      {
        if ((double) num > (double) t1)
          return false;
        if ((double) num > (double) t0)
          t0 = num;
      }
      else if ((double) p > 0.0)
      {
        if ((double) num < (double) t0)
          return false;
        if ((double) num < (double) t1)
          t1 = num;
      }
      else if ((double) q < 0.0)
        return false;
      return true;
    }

    public static Texture2D ColorToTex(int pxSize, Color col)
    {
      Texture2D texture2D = new Texture2D(pxSize, pxSize);
      texture2D.name = nameof (RTEditorGUI);
      for (int x = 0; x < pxSize; ++x)
      {
        for (int y = 0; y < pxSize; ++y)
          texture2D.SetPixel(x, y, col);
      }
      texture2D.Apply();
      return texture2D;
    }

    public static Texture2D Tint(Texture2D tex, Color color)
    {
      Texture2D texture2D = UnityEngine.Object.Instantiate<Texture2D>(tex);
      for (int x = 0; x < tex.width; ++x)
      {
        for (int y = 0; y < tex.height; ++y)
          texture2D.SetPixel(x, y, tex.GetPixel(x, y) * color);
      }
      texture2D.Apply();
      return texture2D;
    }

    public static Texture2D RotateTextureCCW(Texture2D tex, int quarterSteps)
    {
      if ((UnityEngine.Object) tex == (UnityEngine.Object) null)
        return (Texture2D) null;
      tex = UnityEngine.Object.Instantiate<Texture2D>(tex);
      int width = tex.width;
      int height = tex.height;
      Color[] pixels = tex.GetPixels();
      Color[] colorArray = new Color[width * height];
      for (int index1 = 0; index1 < quarterSteps; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          for (int index3 = 0; index3 < height; ++index3)
            colorArray[index2 * width + index3] = pixels[(width - index3 - 1) * width + index2];
        }
        colorArray.CopyTo((Array) pixels, 0);
      }
      tex.SetPixels(pixels);
      tex.Apply();
      return tex;
    }
  }
}
