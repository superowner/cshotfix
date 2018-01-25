using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	void Test0()
    {
        Debug.Log("Unit:void Test0()");
    }
    static FunctionDelegate.method_delegate0 aaa;
    void Test1(int a)
    {
        aaa(this);
        Debug.Log("Unit:void Test1(int a)");
    }
    static void Test2(int a)
    {
        Debug.Log("Unit:static void Test2(int a)");
    }

    public static float[] test3(long a, ref int b, out float c, byte[] data, float d=0)
    {
        c = 0;
        return new float[] { 1 };
    }
}
