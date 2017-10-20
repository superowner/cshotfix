//======================================================================
//   
//        created by lichunlin
//        qq:576067421
//        git:https://github.com/lichunlincn/cshotfix
//
//======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HotFix
{
    public class HotFixLoop : IGameHotFixInterface
    {
        private static HotFixLoop m_Instance;
        public override void Start()
        {

        }
        public override bool Update(float dt)
        {
            Debug.Log("HotFixLoop code update");
            return true;
        }
        public static HotFixLoop GetInstance()
        {
            return m_Instance;
        }

        public override void OnDestroy()
        {


        }
        public override void OnApplicationQuit()
        {

        }
        public override object OnMono2GameDll(string func, params object[] data)
        {
            return null;
        }
    }
}
