// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Helpers.ExpressionExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace YamlDotNet.Helpers
{
  public static class ExpressionExtensions
  {
    public static PropertyInfo AsProperty(this LambdaExpression propertyAccessor)
    {
      PropertyInfo memberExpression = ExpressionExtensions.TryGetMemberExpression<PropertyInfo>(propertyAccessor);
      return !(memberExpression == (PropertyInfo) null) ? memberExpression : throw new ArgumentException("Expected a lambda expression in the form: x => x.SomeProperty", nameof (propertyAccessor));
    }

    private static TMemberInfo TryGetMemberExpression<TMemberInfo>(LambdaExpression lambdaExpression) where TMemberInfo : MemberInfo
    {
      if (lambdaExpression.Parameters.Count != 1)
        return default (TMemberInfo);
      Expression expression = lambdaExpression.Body;
      if (expression is UnaryExpression unaryExpression)
      {
        if (unaryExpression.NodeType != ExpressionType.Convert)
          return default (TMemberInfo);
        expression = unaryExpression.Operand;
      }
      if (!(expression is MemberExpression memberExpression))
        return default (TMemberInfo);
      return memberExpression.Expression != lambdaExpression.Parameters[0] ? default (TMemberInfo) : memberExpression.Member as TMemberInfo;
    }
  }
}
