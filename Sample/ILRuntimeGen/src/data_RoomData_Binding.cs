using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class data_RoomData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(data_RoomData);
            args = new Type[]{typeof(WfPacket)};
            method = type.GetMethod("Serialize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Serialize_0);
            args = new Type[]{typeof(WfPacket)};
            method = type.GetMethod("DeSerialize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DeSerialize_1);

            field = type.GetField("m_roomid", flag);
            app.RegisterCLRFieldGetter(field, get_m_roomid_0);
            app.RegisterCLRFieldSetter(field, set_m_roomid_0);
            field = type.GetField("m_roomname", flag);
            app.RegisterCLRFieldGetter(field, get_m_roomname_1);
            app.RegisterCLRFieldSetter(field, set_m_roomname_1);
            field = type.GetField("m_playerNum", flag);
            app.RegisterCLRFieldGetter(field, get_m_playerNum_2);
            app.RegisterCLRFieldSetter(field, set_m_playerNum_2);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* Serialize_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            WfPacket w = (WfPacket)typeof(WfPacket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            data_RoomData instance_of_this_method;
            instance_of_this_method = (data_RoomData)typeof(data_RoomData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Serialize(w);

            return __ret;
        }

        static StackObject* DeSerialize_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            WfPacket r = (WfPacket)typeof(WfPacket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            data_RoomData instance_of_this_method;
            instance_of_this_method = (data_RoomData)typeof(data_RoomData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.DeSerialize(r);

            return __ret;
        }


        static object get_m_roomid_0(ref object o)
        {
            return ((data_RoomData)o).m_roomid;
        }
        static void set_m_roomid_0(ref object o, object v)
        {
            ((data_RoomData)o).m_roomid = (System.UInt32)v;
        }
        static object get_m_roomname_1(ref object o)
        {
            return ((data_RoomData)o).m_roomname;
        }
        static void set_m_roomname_1(ref object o, object v)
        {
            ((data_RoomData)o).m_roomname = (System.String)v;
        }
        static object get_m_playerNum_2(ref object o)
        {
            return ((data_RoomData)o).m_playerNum;
        }
        static void set_m_playerNum_2(ref object o, object v)
        {
            ((data_RoomData)o).m_playerNum = (System.Byte)v;
        }

        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new data_RoomData();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
