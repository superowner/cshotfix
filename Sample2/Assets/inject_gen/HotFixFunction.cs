//======================================================================
//                                                                       
//        created by lichunlin                                           
//        qq:576067421                                                   
//        git:https://github.com/lichunlincn/cshotfix                    
//                                                                       
//====================================================================== 
using System.Collections; 
using System; 
using HotFix.HotFixDelegate; 
namespace HotFix
{
       public class HotFixFunction
       {
          public static delegate_0 hotfix_func0_delegate;
          public static  void  hotfix_func0(object _this)
         {
                hotfix_func0_delegate(_this);
         }
          public static delegate_0 hotfix_func1_delegate;
          public static  void  hotfix_func1(object _this)
         {
                hotfix_func1_delegate(_this);
         }
          public static delegate_2 hotfix_func2_delegate;
          public static void hotfix_func2(object _this,Int32 _param0_Int32)
         {
                hotfix_func2_delegate(_this,  _param0_Int32);
         }
          public static delegate_3 hotfix_func3_delegate;
          public static Int32 hotfix_func3(object _this,Single _param0_Single, ref UnityEngine.Vector3 _param1_Vector3,String _param2_String, ref System.Int32 _param3_Int32, ref LCL.Logic.ClassData _param4_ClassData, out System.String _param5_String)
         {
               return hotfix_func3_delegate(_this,  _param0_Single,  ref  _param1_Vector3,  _param2_String,  ref  _param3_Int32,  ref  _param4_ClassData,  out  _param5_String);
         }
       }
}
