using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LCL
{
    public class MainTest : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
        }
        void Test0()
        {
            Debug.Log("Unit:void Test0()");
        }
        void Test1(int a)
        {
            Debug.Log("Unit:void Test1(int a)");
        }

        static void Test2(int a)
        {
            Debug.Log("Unit:static void Test2(int a)");
        }

        //暂时不支持4个以上的参数，这个取决于ILRT那边
        public static float[] test3(ref int b, out float c)
        {
            c = 0;
            Debug.LogError("我是一个错误");
            return new float[] { 1 };
        }

        public DataClass test4(DataClass data)
        {
            return data;
        }

    }
}