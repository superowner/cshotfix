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
    unsafe class WindowOption_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(WindowOption);

            field = type.GetField("m_OpenTimeDestroy", flag);
            app.RegisterCLRFieldGetter(field, get_m_OpenTimeDestroy_0);
            app.RegisterCLRFieldSetter(field, set_m_OpenTimeDestroy_0);
            field = type.GetField("m_TimeoutDestroy", flag);
            app.RegisterCLRFieldGetter(field, get_m_TimeoutDestroy_1);
            app.RegisterCLRFieldSetter(field, set_m_TimeoutDestroy_1);
            field = type.GetField("m_WindowLayer", flag);
            app.RegisterCLRFieldGetter(field, get_m_WindowLayer_2);
            app.RegisterCLRFieldSetter(field, set_m_WindowLayer_2);
            field = type.GetField("m_Rejections", flag);
            app.RegisterCLRFieldGetter(field, get_m_Rejections_3);
            app.RegisterCLRFieldSetter(field, set_m_Rejections_3);
            field = type.GetField("m_Dependence", flag);
            app.RegisterCLRFieldGetter(field, get_m_Dependence_4);
            app.RegisterCLRFieldSetter(field, set_m_Dependence_4);
            field = type.GetField("m_Group", flag);
            app.RegisterCLRFieldGetter(field, get_m_Group_5);
            app.RegisterCLRFieldSetter(field, set_m_Group_5);


        }



        static object get_m_OpenTimeDestroy_0(ref object o)
        {
            return ((WindowOption)o).m_OpenTimeDestroy;
        }
        static void set_m_OpenTimeDestroy_0(ref object o, object v)
        {
            ((WindowOption)o).m_OpenTimeDestroy = (System.Boolean)v;
        }
        static object get_m_TimeoutDestroy_1(ref object o)
        {
            return ((WindowOption)o).m_TimeoutDestroy;
        }
        static void set_m_TimeoutDestroy_1(ref object o, object v)
        {
            ((WindowOption)o).m_TimeoutDestroy = (System.Single)v;
        }
        static object get_m_WindowLayer_2(ref object o)
        {
            return ((WindowOption)o).m_WindowLayer;
        }
        static void set_m_WindowLayer_2(ref object o, object v)
        {
            ((WindowOption)o).m_WindowLayer = (WindowLayer)v;
        }
        static object get_m_Rejections_3(ref object o)
        {
            return ((WindowOption)o).m_Rejections;
        }
        static void set_m_Rejections_3(ref object o, object v)
        {
            ((WindowOption)o).m_Rejections = (System.Collections.Generic.List<System.Int32>)v;
        }
        static object get_m_Dependence_4(ref object o)
        {
            return ((WindowOption)o).m_Dependence;
        }
        static void set_m_Dependence_4(ref object o, object v)
        {
            ((WindowOption)o).m_Dependence = (System.Collections.Generic.List<System.Int32>)v;
        }
        static object get_m_Group_5(ref object o)
        {
            return ((WindowOption)o).m_Group;
        }
        static void set_m_Group_5(ref object o, object v)
        {
            ((WindowOption)o).m_Group = (System.Int32)v;
        }


    }
}
