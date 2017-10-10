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
    unsafe class GameDll_PlayerEnterParam_EP_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GameDll.PlayerEnterParam_EP);

            field = type.GetField("m_PlayerId", flag);
            app.RegisterCLRFieldGetter(field, get_m_PlayerId_0);
            app.RegisterCLRFieldSetter(field, set_m_PlayerId_0);


        }



        static object get_m_PlayerId_0(ref object o)
        {
            return ((GameDll.PlayerEnterParam_EP)o).m_PlayerId;
        }
        static void set_m_PlayerId_0(ref object o, object v)
        {
            ((GameDll.PlayerEnterParam_EP)o).m_PlayerId = (System.UInt32)v;
        }


    }
}
