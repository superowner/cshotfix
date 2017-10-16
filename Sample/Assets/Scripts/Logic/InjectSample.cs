using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCL
{
    namespace Logic
    {
        public class InjectSample
        {
            public InjectSample()
            {

            }
            public int TestInject(float a, string str, ref int refint, out string outstr)
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
