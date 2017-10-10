using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameMono
{
    public class GameLoop
    {

        public GameLoop()
        {
            if(HotFixFunction.__hotfix__GameMono_GameLoop_ctr0 != null )
            {
                HotFixFunction.__hotfix__GameMono_GameLoop_ctr0(this);
                return;
            }
            Debug.Log("GameLoop New");
        }
        public int HelloWorld(string helloworld, int data, out float f)
        {
            if(HotFixFunction.__hotfix__GameMono__GameLoop__HelloWorld0 != null )
            {
                return HotFixFunction.__hotfix__GameMono__GameLoop__HelloWorld0(this, helloworld, data, out f);
            }
            f = 0.001f;
            Debug.Log(helloworld);
            return data;
        }
    }
}
