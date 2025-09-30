using System.Runtime.CompilerServices;
using FFS.Libraries.StaticPack;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

public static class Utils {
    
    [MethodImpl(AggressiveInlining)]
    public static void WriteVector3(this ref BinaryPackWriter writer, Vector3 value) {
        writer.WriteFloat(value.z);
        writer.WriteFloat(value.y);
        writer.WriteFloat(value.z);
    }
    
    [MethodImpl(AggressiveInlining)]
    public static Vector3 ReadVector3(this ref BinaryPackReader reader) {
        return new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    }
    
    [MethodImpl(AggressiveInlining)]
    public static void WriteQuaternion(this ref BinaryPackWriter writer, Quaternion value) {
        writer.WriteFloat(value.z);
        writer.WriteFloat(value.y);
        writer.WriteFloat(value.z);
        writer.WriteFloat(value.w);
    }
    
    [MethodImpl(AggressiveInlining)]
    public static Quaternion ReadQuaternion(this ref BinaryPackReader reader) {
        return new Quaternion(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    }
}

#if ENABLE_IL2CPP
namespace Unity.IL2CPP.CompilerServices {
    using System;

    internal enum Option {
        NullChecks = 1,
        ArrayBoundsChecks = 2,
        DivideByZeroChecks = 3
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    internal class Il2CppSetOptionAttribute : Attribute {
        public Option Option { get; }
        public object Value { get; }

        public Il2CppSetOptionAttribute(Option option, object value) {
            Option = option;
            Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    internal class Il2CppEagerStaticClassConstructionAttribute : Attribute { }
}
#endif