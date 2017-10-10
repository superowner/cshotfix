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
    unsafe class Setting_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Setting);

            field = type.GetField("SingleServer", flag);
            app.RegisterCLRFieldGetter(field, get_SingleServer_0);
            app.RegisterCLRFieldSetter(field, set_SingleServer_0);
            field = type.GetField("UseLocalServer", flag);
            app.RegisterCLRFieldGetter(field, get_UseLocalServer_1);
            app.RegisterCLRFieldSetter(field, set_UseLocalServer_1);


        }



        static object get_SingleServer_0(ref object o)
        {
            return ((Setting)o).SingleServer;
        }
        static void set_SingleServer_0(ref object o, object v)
        {
            ((Setting)o).SingleServer = (System.Boolean)v;
        }
        static object get_UseLocalServer_1(ref object o)
        {
            return ((Setting)o).UseLocalServer;
        }
        static void set_UseLocalServer_1(ref object o, object v)
        {
            ((Setting)o).UseLocalServer = (System.Boolean)v;
        }


    }
}
