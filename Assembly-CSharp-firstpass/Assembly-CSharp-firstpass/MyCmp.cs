// Decompiled with JetBrains decompiler
// Type: MyCmp
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MyCmp
{
  private static Dictionary<System.Type, MyCmp.FieldData[]> typeFieldInfos;

  private static void GetFieldDatas(List<MyCmp.FieldData> field_data_list, System.Type type)
  {
    foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
      foreach (object customAttribute in field.GetCustomAttributes(false))
      {
        bool flag1 = customAttribute.GetType() == typeof (MyCmpAdd);
        bool flag2 = customAttribute.GetType() == typeof (MyCmpReq);
        bool flag3 = customAttribute.GetType() == typeof (MyCmpGet);
        if (flag1 | flag2 | flag3)
        {
          bool flag4 = true;
          foreach (MyCmp.FieldData fieldData in field_data_list)
          {
            if (fieldData.fieldInfo.Name == field.Name)
            {
              flag4 = false;
              break;
            }
          }
          if (flag4)
          {
            MyCmp.FieldData fieldData = new MyCmp.FieldData();
            if (flag1)
              fieldData.myCmpType = MyCmp.MyCmpType.Add;
            else if (flag2)
              fieldData.myCmpType = MyCmp.MyCmpType.Req;
            else if (flag3)
              fieldData.myCmpType = MyCmp.MyCmpType.Get;
            fieldData.cmpFns = CmpUtil.GetCmpFns(field.FieldType);
            fieldData.fieldInfo = field;
            field_data_list.Add(fieldData);
          }
        }
      }
    }
    System.Type baseType = type.BaseType;
    if (!(baseType != typeof (KMonoBehaviour)))
      return;
    MyCmp.GetFieldDatas(field_data_list, baseType);
  }

  public static MyCmp.FieldData[] GetFields(System.Type type)
  {
    if (MyCmp.typeFieldInfos == null)
      MyCmp.typeFieldInfos = new Dictionary<System.Type, MyCmp.FieldData[]>();
    MyCmp.FieldData[] fieldDataArray = (MyCmp.FieldData[]) null;
    if (!MyCmp.typeFieldInfos.TryGetValue(type, out fieldDataArray))
    {
      List<MyCmp.FieldData> field_data_list = new List<MyCmp.FieldData>();
      MyCmp.GetFieldDatas(field_data_list, type);
      fieldDataArray = field_data_list.ToArray();
      MyCmp.typeFieldInfos[type] = fieldDataArray;
    }
    return fieldDataArray;
  }

  public static void OnAwake(KMonoBehaviour c)
  {
    foreach (MyCmp.FieldData field in MyCmp.GetFields(c.GetType()))
    {
      CmpFns cmpFns = field.cmpFns;
      FieldInfo fieldInfo = field.fieldInfo;
      if (!((UnityEngine.Object) fieldInfo.GetValue((object) c) != (UnityEngine.Object) null))
      {
        if (field.myCmpType == MyCmp.MyCmpType.Add)
        {
          Component component = cmpFns.mFindOrAddFn(c);
          fieldInfo.SetValue((object) c, (object) component);
        }
        else if (field.myCmpType == MyCmp.MyCmpType.Req)
        {
          Component component = cmpFns.mFindFn(c);
          fieldInfo.SetValue((object) c, (object) component);
        }
        else if (field.myCmpType == MyCmp.MyCmpType.Get)
        {
          Component component = cmpFns.mFindFn(c);
          fieldInfo.SetValue((object) c, (object) component);
        }
      }
    }
  }

  public static void OnStart(KMonoBehaviour c)
  {
    System.Type type = c.GetType();
    foreach (MyCmp.FieldData field in MyCmp.GetFields(type))
    {
      CmpFns cmpFns = field.cmpFns;
      FieldInfo fieldInfo = field.fieldInfo;
      if ((UnityEngine.Object) fieldInfo.GetValue((object) c) != (UnityEngine.Object) null)
        Util.SpawnComponent(fieldInfo.GetValue((object) c) as Component);
      else if (field.myCmpType == MyCmp.MyCmpType.Add)
        Util.SpawnComponent(cmpFns.mFindOrAddFn(c));
      else if (field.myCmpType == MyCmp.MyCmpType.Req)
      {
        Component cmp = cmpFns.mRequireFn(c);
        if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
          Debug.LogError((object) ("The behaviour " + type.ToString() + " required but couldn't find a " + fieldInfo.FieldType.Name));
        Util.SpawnComponent(cmp);
        fieldInfo.SetValue((object) c, (object) cmp);
      }
      else if (field.myCmpType == MyCmp.MyCmpType.Get)
      {
        Component cmp = cmpFns.mFindFn(c);
        Util.SpawnComponent(cmp);
        fieldInfo.SetValue((object) c, (object) cmp);
      }
    }
  }

  public enum MyCmpType
  {
    Req,
    Add,
    Get,
  }

  public class FieldData
  {
    public MyCmp.MyCmpType myCmpType;
    public CmpFns cmpFns;
    public FieldInfo fieldInfo;
  }
}
