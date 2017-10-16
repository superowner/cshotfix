//======================================================================
//   
//        created by lichunlin
//        qq:576067421
//        git:https://github.com/lichunlincn/cshotfix
//
//======================================================================
using UnityEngine;
using System.Collections;

namespace LCL
{
    public class App : MonoBehaviour
    {
        HotFixEngine m_HotFixEngine;
        // Use this for initialization
        void Start()
        {
            m_HotFixEngine = new HotFixEngine();
            m_HotFixEngine.Init();

        }

        // Update is called once per frame
        void Update()
        {
            if(m_HotFixEngine != null)
            {
                m_HotFixEngine.Update();
            }
        }
        private void OnDestroy()
        {
            if(m_HotFixEngine != null)
            {
                m_HotFixEngine.Destroy();
                m_HotFixEngine = null;
            }
        }
    }
}
