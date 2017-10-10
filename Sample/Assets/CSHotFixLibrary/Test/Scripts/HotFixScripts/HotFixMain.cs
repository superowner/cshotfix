using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using GameMono;
using HotFix;

public class HotFixMain
{

	public void Init ()
    {
        HotFixFunction.__hotfix__GameMono_GameLoop_ctr0 = ((object _this) =>
        {
            GameLoop loop = _this as GameLoop;
            Debug.Log("HotFix GameLoop New");
        });

        HotFixFunction.__hotfix__GameMono__GameLoop__HelloWorld0 = ((object _this, string h, int d, out float f)=>
        {
            GameLoop loop = _this as GameLoop;
            f = 1.001f;
            Debug.Log("HotFix GameLoop HelloWorld " + h);

            return d*2;
        });
	}



    public void Update ()
    {
	
	}

    public void Uninit()
    {

    }
}
