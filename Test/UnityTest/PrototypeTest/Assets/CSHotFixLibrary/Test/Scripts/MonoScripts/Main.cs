using UnityEngine;
using System.Collections;
using GameMono;

public class Main : MonoBehaviour
{
    public bool OpenHotFix = false;
	void Start ()
    {
        HotFixMain hotFixEngine = new HotFixMain();
        if(OpenHotFix)
        {
            hotFixEngine.Init();
        }
        else
        {
            hotFixEngine.Uninit();
        }

        GameLoop loop = new GameLoop();
        float f = 0;
        loop.HelloWorld("mono", 13, out f);
	}
}
