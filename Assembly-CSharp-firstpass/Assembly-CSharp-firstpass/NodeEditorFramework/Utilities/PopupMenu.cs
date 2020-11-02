// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Utilities.PopupMenu
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework.Utilities
{
  public class PopupMenu
  {
    public List<PopupMenu.MenuItem> menuItems = new List<PopupMenu.MenuItem>();
    private Rect position;
    private string selectedPath;
    private PopupMenu.MenuItem groupToDraw;
    private float currentItemHeight;
    private bool close;
    public static GUIStyle backgroundStyle;
    public static Texture2D expandRight;
    public static float itemHeight;
    public static GUIStyle selectedLabel;
    public float minWidth;

    public PopupMenu() => this.SetupGUI();

    public void SetupGUI()
    {
      PopupMenu.backgroundStyle = new GUIStyle(GUI.skin.box);
      PopupMenu.backgroundStyle.contentOffset = new Vector2(2f, 2f);
      PopupMenu.expandRight = ResourceManager.LoadTexture("Textures/expandRight.png");
      PopupMenu.itemHeight = GUI.skin.label.CalcHeight(new GUIContent("text"), 100f);
      PopupMenu.selectedLabel = new GUIStyle(GUI.skin.label);
      PopupMenu.selectedLabel.normal.background = RTEditorGUI.ColorToTex(1, new Color(0.4f, 0.4f, 0.4f));
    }

    public void Show(Vector2 pos, float MinWidth = 40f)
    {
      this.minWidth = MinWidth;
      this.position = PopupMenu.calculateRect(pos, this.menuItems, this.minWidth);
      this.selectedPath = "";
      OverlayGUI.currentPopup = this;
    }

    public Vector2 Position => this.position.position;

    public void AddItem(
      GUIContent content,
      bool on,
      PopupMenu.MenuFunctionData func,
      object userData)
    {
      string path;
      PopupMenu.MenuItem menuItem = this.AddHierarchy(ref content, out path);
      if (menuItem != null)
        menuItem.subItems.Add(new PopupMenu.MenuItem(path, content, func, userData));
      else
        this.menuItems.Add(new PopupMenu.MenuItem(path, content, func, userData));
    }

    public void AddItem(GUIContent content, bool on, PopupMenu.MenuFunction func)
    {
      string path;
      PopupMenu.MenuItem menuItem = this.AddHierarchy(ref content, out path);
      if (menuItem != null)
        menuItem.subItems.Add(new PopupMenu.MenuItem(path, content, func));
      else
        this.menuItems.Add(new PopupMenu.MenuItem(path, content, func));
    }

    public void AddSeparator(string path)
    {
      GUIContent content = new GUIContent(path);
      PopupMenu.MenuItem menuItem = this.AddHierarchy(ref content, out path);
      if (menuItem != null)
        menuItem.subItems.Add(new PopupMenu.MenuItem());
      else
        this.menuItems.Add(new PopupMenu.MenuItem());
    }

    private PopupMenu.MenuItem AddHierarchy(ref GUIContent content, out string path)
    {
      path = content.text;
      if (!path.Contains("/"))
        return (PopupMenu.MenuItem) null;
      string[] strArray = path.Split('/');
      string folderPath = strArray[0];
      PopupMenu.MenuItem menuItem1 = this.menuItems.Find((Predicate<PopupMenu.MenuItem>) (item => item.content != null && item.content.text == folderPath && item.group));
      if (menuItem1 == null)
        this.menuItems.Add(menuItem1 = new PopupMenu.MenuItem(folderPath, new GUIContent(folderPath), true));
      for (int index = 1; index < strArray.Length - 1; ++index)
      {
        string folder = strArray[index];
        folderPath = folderPath + "/" + folder;
        if (menuItem1 == null)
          Debug.LogError((object) "Parent is null!");
        else if (menuItem1.subItems == null)
          Debug.LogError((object) ("Subitems of " + menuItem1.content.text + " is null!"));
        PopupMenu.MenuItem menuItem2 = menuItem1.subItems.Find((Predicate<PopupMenu.MenuItem>) (item => item.content != null && item.content.text == folder && item.group));
        if (menuItem2 == null)
          menuItem1.subItems.Add(menuItem2 = new PopupMenu.MenuItem(folderPath, new GUIContent(folder), true));
        menuItem1 = menuItem2;
      }
      path = content.text;
      content = new GUIContent(strArray[strArray.Length - 1], content.tooltip);
      return menuItem1;
    }

    public void Draw()
    {
      bool flag = this.DrawGroup(this.position, this.menuItems);
      while (this.groupToDraw != null && !this.close)
      {
        PopupMenu.MenuItem groupToDraw = this.groupToDraw;
        this.groupToDraw = (PopupMenu.MenuItem) null;
        if (groupToDraw.group && this.DrawGroup(groupToDraw.groupPos, groupToDraw.subItems))
          flag = true;
      }
      if (!flag || this.close)
        OverlayGUI.currentPopup = (PopupMenu) null;
      NodeEditor.RepaintClients();
    }

    private bool DrawGroup(Rect pos, List<PopupMenu.MenuItem> menuItems)
    {
      Rect rect1 = PopupMenu.calculateRect(pos.position, menuItems, this.minWidth);
      Rect rect2 = new Rect(rect1);
      rect2.xMax += 20f;
      rect2.xMin -= 20f;
      rect2.yMax += 20f;
      rect2.yMin -= 20f;
      bool flag = rect2.Contains(UnityEngine.Event.current.mousePosition);
      this.currentItemHeight = PopupMenu.backgroundStyle.contentOffset.y;
      GUI.BeginGroup(PopupMenu.extendRect(rect1, PopupMenu.backgroundStyle.contentOffset), GUIContent.none, PopupMenu.backgroundStyle);
      for (int index = 0; index < menuItems.Count; ++index)
      {
        this.DrawItem(menuItems[index], rect1);
        if (this.close)
          break;
      }
      GUI.EndGroup();
      return flag;
    }

    private void DrawItem(PopupMenu.MenuItem item, Rect groupRect)
    {
      if (item.separator)
      {
        if (UnityEngine.Event.current.type == EventType.Repaint)
          RTEditorGUI.Seperator(new Rect(PopupMenu.backgroundStyle.contentOffset.x + 1f, this.currentItemHeight + 1f, groupRect.width - 2f, 1f));
        this.currentItemHeight += 3f;
      }
      else
      {
        Rect position = new Rect(PopupMenu.backgroundStyle.contentOffset.x, this.currentItemHeight, groupRect.width, PopupMenu.itemHeight);
        if (position.Contains(UnityEngine.Event.current.mousePosition))
          this.selectedPath = item.path;
        bool flag = this.selectedPath == item.path || this.selectedPath.Contains(item.path + "/");
        GUI.Label(position, item.content, flag ? PopupMenu.selectedLabel : GUI.skin.label);
        if (item.group)
        {
          GUI.DrawTexture(new Rect((float) ((double) position.x + (double) position.width - 12.0), position.y + (float) (((double) position.height - 12.0) / 2.0), 12f, 12f), (Texture) PopupMenu.expandRight);
          if (flag)
          {
            item.groupPos = new Rect((float) ((double) groupRect.x + (double) groupRect.width + 4.0), (float) ((double) groupRect.y + (double) this.currentItemHeight - 2.0), 0.0f, 0.0f);
            this.groupToDraw = item;
          }
        }
        else if (flag && (UnityEngine.Event.current.type == EventType.MouseDown || UnityEngine.Event.current.button != 1 && UnityEngine.Event.current.type == EventType.MouseUp))
        {
          item.Execute();
          this.close = true;
          UnityEngine.Event.current.Use();
        }
        this.currentItemHeight += PopupMenu.itemHeight;
      }
    }

    private static Rect extendRect(Rect rect, Vector2 extendValue)
    {
      rect.x -= extendValue.x;
      rect.y -= extendValue.y;
      rect.width += extendValue.x + extendValue.x;
      rect.height += extendValue.y + extendValue.y;
      return rect;
    }

    private static Rect calculateRect(
      Vector2 position,
      List<PopupMenu.MenuItem> menuItems,
      float minWidth)
    {
      float num = minWidth;
      float y = 0.0f;
      for (int index = 0; index < menuItems.Count; ++index)
      {
        PopupMenu.MenuItem menuItem = menuItems[index];
        if (menuItem.separator)
        {
          y += 3f;
        }
        else
        {
          num = Mathf.Max(num, GUI.skin.label.CalcSize(menuItem.content).x + (menuItem.group ? 22f : 10f));
          y += PopupMenu.itemHeight;
        }
      }
      Vector2 vector2 = new Vector2(num, y);
      bool flag = (double) position.y + (double) vector2.y <= (double) Screen.height;
      return new Rect(position.x, position.y - (flag ? 0.0f : vector2.y), vector2.x, vector2.y);
    }

    public delegate void MenuFunction();

    public delegate void MenuFunctionData(object userData);

    public class MenuItem
    {
      public string path;
      public GUIContent content;
      public PopupMenu.MenuFunction func;
      public PopupMenu.MenuFunctionData funcData;
      public object userData;
      public bool separator;
      public bool group;
      public Rect groupPos;
      public List<PopupMenu.MenuItem> subItems;

      public MenuItem() => this.separator = true;

      public MenuItem(string _path, GUIContent _content, bool _group)
      {
        this.path = _path;
        this.content = _content;
        this.group = _group;
        if (!this.group)
          return;
        this.subItems = new List<PopupMenu.MenuItem>();
      }

      public MenuItem(string _path, GUIContent _content, PopupMenu.MenuFunction _func)
      {
        this.path = _path;
        this.content = _content;
        this.func = _func;
      }

      public MenuItem(
        string _path,
        GUIContent _content,
        PopupMenu.MenuFunctionData _func,
        object _userData)
      {
        this.path = _path;
        this.content = _content;
        this.funcData = _func;
        this.userData = _userData;
      }

      public void Execute()
      {
        if (this.funcData != null)
        {
          this.funcData(this.userData);
        }
        else
        {
          if (this.func == null)
            return;
          this.func();
        }
      }
    }
  }
}
