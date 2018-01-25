using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class GameMain
    {
        //public static FunctionDelegate.method_delegate0 m_TestRef_Delegate;
        public void TestRef(ref int arg0, ref int arg1)
        {
            //if(m_TestRef_Delegate!= null)
            //{
            //    m_TestRef_Delegate(ref arg0);
            //}
        }
        public void TestRef2(int arg0, ref int arg1)
        {

        }

        public void TestOut(int arg0, out int arg1)
        {
            arg1 = 1;
        }

        public float TestRefOut(int arg0,ref float arg1, out int arg2)
        {
            arg1 = 1;
            arg2 = 1;
            return arg1;
        }
        public void TestArray(int[] arg0)
        {

        }

        public float TestArray1(int[] arg0, ref int arg1, out float arg2, UInt64 arg3)
        {
            arg2 = 1;
            return arg2;
        }
        public float[] TestReturnArray()
        {
            return new float[] { 1 };
        }


    }
}
