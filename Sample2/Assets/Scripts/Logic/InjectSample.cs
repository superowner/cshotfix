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
                int a = 0;
                Debug.Log(a);
            }
            public int TestInject(float a, ref Vector3 v3, string str, ref int refint, ref ClassData data , out string outstr)
            {
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
