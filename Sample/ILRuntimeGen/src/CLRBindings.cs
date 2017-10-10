using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {
        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            GameDll_DebugManager_Binding.Register(app);
            System_Collections_Generic_List_1_Action_1_ILTypeInstance_Binding.Register(app);
            System_Action_1_ILTypeInstance_Binding.Register(app);
            System_UInt32_Binding.Register(app);
            System_String_Binding.Register(app);
            GameDll_Tool_Binding.Register(app);
            ResourceManager_Binding.Register(app);
            System_IO_Path_Binding.Register(app);
            GameDll_StaticEventParam_Binding.Register(app);
            GameDll_StartLoadLevel_EP_Binding.Register(app);
            GameDll_CGameProcedure_Binding.Register(app);
            GameDll_EventManager_Binding.Register(app);
            System_Collections_Generic_List_1_data_RoomData_Binding.Register(app);
            data_RoomData_Binding.Register(app);
            GameDll_PlayerManager_Binding.Register(app);
            GameDll_IPlayer_Binding.Register(app);
            GameDll_PlayerEnterParam_EP_Binding.Register(app);
            GameDll_NetHelper_Binding.Register(app);
            System_Int32_Binding.Register(app);
            GameDll_LogicRoot_Binding.Register(app);
            Setting_Binding.Register(app);
            NetWrapper_Binding.Register(app);
            System_Boolean_Binding.Register(app);
            GameDll_DataManager_Binding.Register(app);
            System_UInt16_Binding.Register(app);
            System_Object_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            GameDll_BattleProgress_Binding.Register(app);
            GameDll_IBattle_Binding.Register(app);
            System_Collections_Hashtable_Binding.Register(app);
            WfPacket_Binding.Register(app);
            Net_Binding.Register(app);
            System_Convert_Binding.Register(app);
            System_Action_1_WfPacket_Binding.Register(app);
            MsgStream_Binding.Register(app);
            ISerializePacket_Binding.Register(app);
            System_Collections_Generic_List_1_data_RoomData_Binding_Enumerator_data_RoomData_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_Generic_List_1_data_RoomMemberInfo_Binding.Register(app);
            System_Collections_Generic_List_1_data_RoomMemberInfo_Binding_Enumerator_data_RoomMemberInfo_Binding.Register(app);
            data_RoomMemberInfo_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_GC_Binding.Register(app);
            WindowOption_Binding.Register(app);
            System_Text_StringBuilder_Binding.Register(app);
            MyExtensionMethods_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            ComponentBridge_Binding.Register(app);
            UIEventListener_Binding.Register(app);
            UnityEngine_UI_InputField_Binding.Register(app);
            GameDll_GMManager_Binding.Register(app);
            UnityEngine_UI_Toggle_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Activator_Binding.Register(app);
            UnityEngine_Camera_Binding.Register(app);
            UnityEngine_LayerMask_Binding.Register(app);
            UnityEngine_Canvas_Binding.Register(app);
            UnityEngine_UI_CanvasScaler_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            System_Enum_Binding.Register(app);
            System_Array_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            UnityEngine_RectTransform_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_GameObject_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
        }
    }
}
