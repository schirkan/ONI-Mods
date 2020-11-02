// Decompiled with JetBrains decompiler
// Type: KSerialization.TypeInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace KSerialization
{
  public class TypeInfo
  {
    public Type type;
    public SerializationTypeInfo info;
    public TypeInfo[] subTypes;
    public Type genericInstantiationType;
    public Type[] genericTypeArgs;

    public void BuildGenericArgs()
    {
      if (this.type.IsGenericType)
      {
        Type genericTypeDefinition = this.type.GetGenericTypeDefinition();
        this.genericTypeArgs = this.type.GetGenericArguments();
        this.genericInstantiationType = genericTypeDefinition.MakeGenericType(this.genericTypeArgs);
      }
      if (this.subTypes == null)
        return;
      for (int index = 0; index < this.subTypes.Length; ++index)
        this.subTypes[index].BuildGenericArgs();
    }

    public override bool Equals(object obj) => obj != null && obj is TypeInfo && this.Equals((TypeInfo) obj);

    public bool Equals(TypeInfo other)
    {
      if (this.info != other.info)
        return false;
      if (this.subTypes != null && other.subTypes != null)
      {
        if (this.subTypes.Length != other.subTypes.Length)
          return false;
        for (int index = 0; index < this.subTypes.Length; ++index)
        {
          if (!this.subTypes[index].Equals(other.subTypes[index]))
            return false;
        }
        return true;
      }
      return this.subTypes == null && other.subTypes == null && this.type == other.type;
    }

    public override int GetHashCode() => this.type.GetHashCode();
  }
}
