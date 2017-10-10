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
    unsafe class GameDll_CGameProcedure_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GameDll.CGameProcedure);
            args = new Type[]{typeof(System.Object)};
            method = type.GetMethod("SetProcedureStatus", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetProcedureStatus_0);
            args = new Type[]{typeof(GameDll.CGameProcedure)};
            method = type.GetMethod("SetActiveProc", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetActiveProc_1);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetDisconnect", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDisconnect_2);

            field = type.GetField("s_EventManager", flag);
            app.RegisterCLRFieldGetter(field, get_s_EventManager_0);
            app.RegisterCLRFieldSetter(field, set_s_EventManager_0);
            field = type.GetField("s_PlayerManager", flag);
            app.RegisterCLRFieldGetter(field, get_s_PlayerManager_1);
            app.RegisterCLRFieldSetter(field, set_s_PlayerManager_1);
            field = type.GetField("s_ProcLobby", flag);
            app.RegisterCLRFieldGetter(field, get_s_ProcLobby_2);
            app.RegisterCLRFieldSetter(field, set_s_ProcLobby_2);


        }


        static StackObject* SetProcedureStatus_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object state = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            GameDll.CGameProcedure.SetProcedureStatus(state);

            return __ret;
        }

        static StackObject* SetActiveProc_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GameDll.CGameProcedure toActive = (GameDll.CGameProcedure)typeof(GameDll.CGameProcedure).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            GameDll.CGameProcedure.SetActiveProc(toActive);

            return __ret;
        }

        static StackObject* SetDisconnect_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean disconnect = ptr_of_this_method->Value == 1;

            GameDll.CGameProcedure.SetDisconnect(disconnect);

            return __ret;
        }


        static object get_s_EventManager_0(ref object o)
        {
            return GameDll.CGameProcedure.s_EventManager;
        }
        static void set_s_EventManager_0(ref object o, object v)
        {
            GameDll.CGameProcedure.s_EventManager = (GameDll.EventManager)v;
        }
        static object get_s_PlayerManager_1(ref object o)
        {
            return GameDll.CGameProcedure.s_PlayerManager;
        }
        static void set_s_PlayerManager_1(ref object o, object v)
        {
            GameDll.CGameProcedure.s_PlayerManager = (GameDll.PlayerManager)v;
        }
        static object get_s_ProcLobby_2(ref object o)
        {
            return GameDll.CGameProcedure.s_ProcLobby;
        }
        static void set_s_ProcLobby_2(ref object o, object v)
        {
            GameDll.CGameProcedure.s_ProcLobby = (GameDll.CGamePro_Lobby)v;
        }


    }
}
