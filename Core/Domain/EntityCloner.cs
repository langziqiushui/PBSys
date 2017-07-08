using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace YX.Core
{
    public static class EntityCloner
    {

        delegate object ObjectCloneMethod(object AObject);
        public static T GenericClone<T>(T AObject)
        {
            if (AObject is ValueType || AObject == null)
                return AObject;
            return (T)GetColoneMethod(AObject.GetType())(AObject);
        }

        public static object Clone(object AObject)
        {
            if (AObject is ValueType || AObject == null)
                return AObject;
            return GetColoneMethod(AObject.GetType())(AObject);
        }

        static readonly Hashtable MethodList = new Hashtable();
        static ObjectCloneMethod GetColoneMethod(Type CloneType)
        {
            lock (MethodList)
            {
                if (MethodList.ContainsKey(CloneType.GUID))
                    return (ObjectCloneMethod)MethodList[CloneType.GUID];

                lock (MethodList)
                {
                    Type[] methodArgs = { typeof(object) };
                    DynamicMethod AMethod = new DynamicMethod("", typeof(object), methodArgs, CloneType);
                    ILGenerator il = AMethod.GetILGenerator();
                    ConstructorInfo createInfo = CloneType.GetConstructor(new Type[0]);
                    il.DeclareLocal(CloneType);
                    il.Emit(OpCodes.Newobj, createInfo);
                    il.Emit(OpCodes.Stloc_0);
                    PropertyInfo[] Properties = CloneType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo AProp in Properties)
                    {
                        if (!(AProp.CanWrite && AProp.CanRead))
                            continue;
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Callvirt, AProp.GetGetMethod());
                        il.Emit(OpCodes.Callvirt, AProp.GetSetMethod());
                    }
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ret);
                    ObjectCloneMethod Result = null;
                    try
                    {
                        Result = (ObjectCloneMethod)AMethod.CreateDelegate(typeof(ObjectCloneMethod));
                    }
                    catch (Exception ex)
                    {

                        throw ex;

                    }
                    MethodList.Add(CloneType.GUID, Result);
                    return Result;
                }
            }

        }

    }

}
