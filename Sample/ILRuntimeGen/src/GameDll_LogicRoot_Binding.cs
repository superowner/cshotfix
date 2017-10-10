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
    unsafe class GameDll_LogicRoot_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GameDll.LogicRoot);
            args = new Type[]{};
            method = type.GetMethod("GetSetting", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetSetting_0);

            field = type.GetField("m_ScreenDesignSize", flag);
            app.RegisterCLRFieldGetter(field, get_m_ScreenDesignSize_0);
            app.RegisterCLRFieldSetter(field, set_m_ScreenDesignSize_0);
            field = type.GetField("m_fTimeSinceLastFrame", flag);
            app.RegisterCLRFieldGetter(field, get_m_fTimeSinceLastFrame_1);
            app.RegisterCLRFieldSetter(field, set_m_fTimeSinceLastFrame_1);


        }


        static StackObject* GetSetting_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = GameDll.LogicRoot.GetSetting();

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_m_ScreenDesignSize_0(ref object o)
        {
            return GameDll.LogicRoot.m_ScreenDesignSize;
        }
        static void set_m_ScreenDesignSize_0(ref object o, object v)
        {
            GameDll.LogicRoot.m_ScreenDesignSize = (UnityEngine.Vector2)v;
        }
        static object get_m_fTimeSinceLastFrame_1(ref object o)
        {
            return GameDll.LogicRoot.m_fTimeSinceLastFrame;
        }
        static void set_m_fTimeSinceLastFrame_1(ref object o, object v)
        {
            GameDll.LogicRoot.m_fTimeSinceLastFrame = (System.Single)v;
        }


    }
}
