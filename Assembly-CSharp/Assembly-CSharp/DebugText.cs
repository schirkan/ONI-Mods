﻿// Decompiled with JetBrains decompiler
// Type: DebugText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/DebugText")]
public class DebugText : KMonoBehaviour
{
  public static DebugText Instance;
  private List<DebugText.Entry> entries = new List<DebugText.Entry>();
  private List<Text> texts = new List<Text>();

  public static void DestroyInstance() => DebugText.Instance = (DebugText) null;

  protected override void OnPrefabInit() => DebugText.Instance = this;

  public void Draw(string text, Vector3 pos, Color color) => this.entries.Add(new DebugText.Entry()
  {
    text = text,
    pos = pos,
    color = color
  });

  private void LateUpdate()
  {
    foreach (Component text in this.texts)
      Object.Destroy((Object) text.gameObject);
    this.texts.Clear();
    foreach (DebugText.Entry entry in this.entries)
    {
      GameObject gameObject = new GameObject();
      RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
      rectTransform.SetParent((Transform) GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
      gameObject.transform.SetPosition(entry.pos);
      rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);
      Text text = gameObject.AddComponent<Text>();
      text.font = Assets.DebugFont;
      text.text = entry.text;
      text.color = entry.color;
      text.horizontalOverflow = HorizontalWrapMode.Overflow;
      text.verticalOverflow = VerticalWrapMode.Overflow;
      text.alignment = TextAnchor.MiddleCenter;
      this.texts.Add(text);
    }
    this.entries.Clear();
  }

  private struct Entry
  {
    public string text;
    public Vector3 pos;
    public Color color;
  }
}
