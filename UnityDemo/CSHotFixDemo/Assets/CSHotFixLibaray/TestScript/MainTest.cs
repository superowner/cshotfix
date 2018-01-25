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
    void Test1(int a)
    {
        Debug.Log("Unit:void Test1(int a)");
    }
    static FunctionDelegate.method_delegate1 aaa;
    static void Test2(int a)
    {
        aaa(null,a);
        Debug.Log("Unit:static void Test2(int a)");
    }
}
