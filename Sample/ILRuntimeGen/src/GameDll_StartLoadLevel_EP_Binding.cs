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
    unsafe class GameDll_StartLoadLevel_EP_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GameDll.StartLoadLevel_EP);

            field = type.GetField("m_nLevelId", flag);
            app.RegisterCLRFieldGetter(field, get_m_nLevelId_0);
            app.RegisterCLRFieldSetter(field, set_m_nLevelId_0);


        }



        static object get_m_nLevelId_0(ref object o)
        {
            return ((GameDll.StartLoadLevel_EP)o).m_nLevelId;
        }
        static void set_m_nLevelId_0(ref object o, object v)
        {
            ((GameDll.StartLoadLevel_EP)o).m_nLevelId = (System.Int32)v;
        }


    }
}
