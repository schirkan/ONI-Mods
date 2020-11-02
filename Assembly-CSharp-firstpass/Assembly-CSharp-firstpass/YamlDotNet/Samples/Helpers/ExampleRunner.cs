// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.Helpers.ExampleRunner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace YamlDotNet.Samples.Helpers
{
  public class ExampleRunner : MonoBehaviour
  {
    private ExampleRunner.StringTestOutputHelper helper = new ExampleRunner.StringTestOutputHelper();
    public string[] disabledTests = new string[0];

    public static string[] GetAllTestNames()
    {
      List<string> stringList = new List<string>();
      foreach (System.Type type in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (type.Namespace == "YamlDotNet.Samples" && type.IsClass)
        {
          bool flag = false;
          foreach (MethodInfo method in type.GetMethods())
          {
            if (method.Name == "Main" && (SampleAttribute) Attribute.GetCustomAttribute((MemberInfo) method, typeof (SampleAttribute)) != null)
            {
              stringList.Add(type.Name);
              break;
            }
            if (flag)
              break;
          }
        }
      }
      return stringList.ToArray();
    }

    public static string[] GetAllTestTitles()
    {
      List<string> stringList = new List<string>();
      foreach (System.Type type in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (type.Namespace == "YamlDotNet.Samples" && type.IsClass)
        {
          bool flag = false;
          foreach (MethodInfo method in type.GetMethods())
          {
            if (method.Name == "Main")
            {
              SampleAttribute customAttribute = (SampleAttribute) Attribute.GetCustomAttribute((MemberInfo) method, typeof (SampleAttribute));
              if (customAttribute != null)
              {
                stringList.Add(customAttribute.Title);
                break;
              }
            }
            if (flag)
              break;
          }
        }
      }
      return stringList.ToArray();
    }

    private void Start()
    {
      foreach (System.Type type in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (type.Namespace == "YamlDotNet.Samples" && type.IsClass && Array.IndexOf<string>(this.disabledTests, type.Name) == -1)
        {
          bool flag = false;
          foreach (MethodInfo method in type.GetMethods())
          {
            if (method.Name == "Main")
            {
              SampleAttribute customAttribute = (SampleAttribute) Attribute.GetCustomAttribute((MemberInfo) method, typeof (SampleAttribute));
              if (customAttribute != null)
              {
                this.helper.WriteLine("{0} - {1}", new object[2]
                {
                  (object) customAttribute.Title,
                  (object) customAttribute.Description
                });
                object obj = type.GetConstructor(new System.Type[1]
                {
                  typeof (ExampleRunner.StringTestOutputHelper)
                }).Invoke(new object[1]
                {
                  (object) this.helper
                });
                method.Invoke(obj, new object[0]);
                Debug.Log((object) this.helper.ToString());
                this.helper.Clear();
                break;
              }
            }
            if (flag)
              break;
          }
        }
      }
    }

    private class StringTestOutputHelper : ITestOutputHelper
    {
      private StringBuilder output = new StringBuilder();

      public void WriteLine() => this.output.AppendLine();

      public void WriteLine(string value) => this.output.AppendLine(value);

      public void WriteLine(string format, params object[] args)
      {
        this.output.AppendFormat(format, args);
        this.output.AppendLine();
      }

      public override string ToString() => this.output.ToString();

      public void Clear() => this.output = new StringBuilder();
    }
  }
}
