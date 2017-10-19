using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCL
{
    namespace Logic
    {
        public class ClassData
        {

        }
        public class InjectSample
        {
            public InjectSample()
            {

            }
            public void TestInject2(int c)
            {
                if (HotFix.HotFixFunction.lcl__logic__injectsample__TestInject2__0 != null)
                {
                    HotFix.HotFixFunction.lcl__logic__injectsample__TestInject2__0(this, c);
                    return;
                }
                int a = 0;
                Debug.Log(a);
            }
            public int TestInject(float a, ref Vector3 v3, string str, ref int refint, ref ClassData data , out string outstr)
            {
                if(HotFix.HotFixFunction.lcl__logic__injectsample__TestInject__0 != null)
                {
                    return HotFix.HotFixFunction.lcl__logic__injectsample__TestInject__0(this, a, ref v3, str, ref refint, ref data, out outstr);
                }
                refint = 0;
                outstr = "Mono code";
                Debug.Log("TestInject Mono code");
                return -1;
            }
            [Inject(InjectFlag.NoInject)]
            public void NotInject(string a)
            {
                Debug.Log("TestInject Mono code NotInject");
            }
        }
    }
}
