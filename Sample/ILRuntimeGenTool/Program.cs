using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ILRuntimeGenTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            string injectGen =Path.GetFullPath( "../../../Assets/Plugins/InjectGen.dll");
            string hotdll = Path.GetFullPath("../../../Assets/Out/HotFixDll.dll.bytes");

            string bindroot = Path.GetFullPath("../../../ILRuntimeGen");
            string bindgen = Path.Combine(bindroot, "src");

            string[] oldFiles = System.IO.Directory.GetFiles(bindgen, "*.cs");
            foreach (var i in oldFiles)
            {
                System.IO.File.Delete(i);
            }

            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.ClearClassNames();

            ILRuntime.Runtime.Enviorment.AppDomain domain_injectgen = new ILRuntime.Runtime.Enviorment.AppDomain();
            using (System.IO.FileStream fs = new System.IO.FileStream(injectGen, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                domain_injectgen.LoadAssembly(fs);
            }
            InitILRuntime_InjectGen(domain_injectgen);
            Assembly injectdll = Assembly.Load("InjectGen");
            List<Type> types = injectdll.GetTypes().ToList();
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, bindgen);

            ILRuntime.Runtime.Enviorment.AppDomain domain_hotdll = new ILRuntime.Runtime.Enviorment.AppDomain();
            using (System.IO.FileStream fs = new System.IO.FileStream(hotdll, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                domain_hotdll.LoadAssembly(fs);
            }

            InitILRuntime_HotDll(domain_hotdll);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain_hotdll, bindgen);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateCLRBindingsCode(bindgen);

            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.ClearClassNames();

            AddVSProject(bindroot);

        }

        static void InitILRuntime_InjectGen(ILRuntime.Runtime.Enviorment.AppDomain domain)
        {

        }


        static void InitILRuntime_HotDll(ILRuntime.Runtime.Enviorment.AppDomain domain)
        {
            //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
            //domain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
            //domain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
            domain.RegisterCrossBindingAdaptor(new IGameHotFixInterfaceAdapter());
            //domain.RegisterCrossBindingAdaptor(new ISerializePacketAdapter());
        }


        static void AddVSProject(string name)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string csprojpath = Path.Combine(name, "ILRuntimeGen.csproj");
            xmlDoc.Load(csprojpath);
            XmlNodeList root_childlist = xmlDoc.ChildNodes;
            XmlNode root_Project = null; ;
            foreach (XmlNode xn in root_childlist)
            {
                if (xn.Name == "Project")
                {
                    root_Project = xn;
                    break;
                }
            }
            XmlNodeList childlist_Project = root_Project.ChildNodes;//根节点的字节点
            XmlNode ItemGroup_Compile = null; //编译节点，就是后台代码文件.cs 
            foreach (XmlNode xn in childlist_Project)
            {
                if (xn.Name == "ItemGroup")
                {
                    if (xn.FirstChild.Name == "Compile")
                    {
                        ItemGroup_Compile = xn;//编译节点
                        break;
                    }
                }
            }
            //删除src的节点
            int count = ItemGroup_Compile.ChildNodes.Count;
            for (int i = count - 1; i >= 0; --i)
            {
                XmlElement node_Compile = (XmlElement)ItemGroup_Compile.ChildNodes[i];
                string include = node_Compile.GetAttribute("Include");
                if (include.StartsWith("src\\"))
                {
                    ItemGroup_Compile.RemoveChild(node_Compile);
                }
            }
            string srcpath = Path.Combine(name, "src");
            string[] gen_cs = Directory.GetFiles(srcpath);
            bool node_dirty = false;
            foreach (string strfile in gen_cs)
            {
                XmlElement node_Compile = xmlDoc.CreateElement("Compile", xmlDoc.DocumentElement.NamespaceURI);
                string shortName = Path.GetFileName(strfile);
                string node_name = "src\\" + shortName;

                if (!IsNodeHas(ItemGroup_Compile, node_name))
                {
                    node_Compile.SetAttribute("Include", node_name);
                    ItemGroup_Compile.AppendChild(node_Compile);
                    node_dirty = true;
                }
            }
            if (node_dirty)
            {
                xmlDoc.Save(csprojpath);
            }
            xmlDoc = null;
        }

        private static bool IsNodeHas(XmlNode ItemGroup_Compile, string name)
        {
            //查找是否已经添加了该文件
            int count = ItemGroup_Compile.ChildNodes.Count;
            for (int i = count - 1; i >= 0; --i)
            {
                XmlElement node_Compile = (XmlElement)ItemGroup_Compile.ChildNodes[i];
                string include = node_Compile.GetAttribute("Include");
                if (include.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }














    }
}
