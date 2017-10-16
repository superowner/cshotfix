//======================================================================
//   
//        created by lichunlin
//        qq:576067421
//        git:https://github.com/lichunlincn/cshotfix
//
//======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCL
{
    public class HotFixEngine
    {
        public HotFixEngine()
        {

        }
        private ILRuntime.Runtime.Enviorment.AppDomain m_AssemblyILR = null;
        private IGameHotFixInterface m_HotFixDll = null;

        public void Init()
        {
            if (!Load())
            {
                Debug.LogError("script dll load err");
                return;
            }

            m_AssemblyILR.AllowUnboundCLRMethod = true;
            Debug.LogError("ILRT 检测是否绑定类：true");
            

            //注册跨域类
            m_AssemblyILR.RegisterCrossBindingAdaptor(new IGameHotFixInterfaceAdapter());
            //m_AssemblyILR.RegisterCrossBindingAdaptor(new ISerializePacketAdapter());


            m_AssemblyILR.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
            m_AssemblyILR.DelegateManager.RegisterMethodDelegate<System.Int32>();
            m_AssemblyILR.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            m_AssemblyILR.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();

            m_HotFixDll = m_AssemblyILR.Instantiate<IGameHotFixInterface>("HotFix.HotFixLoop");
            m_HotFixDll.Start();
        }
        private bool Load()
        {
            byte[] dllData = null;
            string dll_path = "";
            if (Application.platform == RuntimePlatform.Android)
            {
                dll_path = Application.dataPath + "!assets/HotFixDll.dll.bytes";

                AssetBundle ab = AssetBundle.LoadFromFile(dll_path);
                if (ab == null)
                {
                    Debug.LogError("load dll failed ,path is:" + dll_path);
                }
                else
                {
                    Debug.Log("load dll ok, path is:" + dll_path);
                    dllData = ab.LoadAsset<TextAsset>("HotFixDll.dll.bytes").bytes;
                }

            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //todo
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string outer_path = Application.dataPath + "/Out/HotFixDll.dll.bytes";
                FileStream fileStream = File.OpenRead(dll_path);
                if (fileStream != null && fileStream.Length > 0)
                {
                    byte[] byteData = new byte[fileStream.Length];
                    fileStream.Read(byteData, 0, (int)fileStream.Length);
                    fileStream.Close();
                    dllData = byteData;
                    Debug.Log("hotfix 版本：包内版本, path:" + dll_path);
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                //todo
            }
            if (dllData == null)
            {
                Debug.LogError("GameDll can not find !");
                return false;
            }

            byte[] pdbData = null;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {

                FileStream fileStream = File.OpenRead(Application.dataPath + "/Out/HotFixDll.dll.pdb");
                if (fileStream != null && fileStream.Length > 0)
                {
                    byte[] byteData = new byte[fileStream.Length];
                    fileStream.Read(byteData, 0, (int)fileStream.Length);
                    fileStream.Close();
                    pdbData = byteData;
                }

            }
            if (dllData != null)
            {

                if (m_AssemblyILR == null)
                {
                    m_AssemblyILR = new ILRuntime.Runtime.Enviorment.AppDomain();
                }

                if (pdbData != null)
                {

                    using (System.IO.MemoryStream fs = new MemoryStream(dllData))
                    {
                        using (System.IO.MemoryStream p = new MemoryStream(pdbData))
                        {
                            m_AssemblyILR.LoadAssembly(fs, p, new Mono.Cecil.Pdb.PdbReaderProvider());
                        }
                    }

                }
                else
                {

                    using (System.IO.MemoryStream fs = new MemoryStream(dllData))
                    {
                        m_AssemblyILR.LoadAssembly(fs);
                    }
                }
            }
            return true;
        }

        public void Update()
        {

        }
        public void Destroy()
        {

        }
    }
}
