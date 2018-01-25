using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class FunctionDelegate
{
     public delegate void method_delegate0(ref System.Int32 arg0,ref System.Int32 arg1);
     public delegate void method_delegate1(System.Int32 arg0,ref System.Int32 arg1);
     public delegate void method_delegate2(System.Int32 arg0,out System.Int32 arg1);
     public delegate System.Single method_delegate3(System.Int32 arg0,ref System.Single arg1,out System.Int32 arg2);
     public delegate void method_delegate4(System.Int32[] arg0);
     public delegate System.Single method_delegate5(System.Int32[] arg0,ref System.Int32 arg1,out System.Single arg2,System.UInt64 arg3);
     public delegate System.Single[] method_delegate6();
}