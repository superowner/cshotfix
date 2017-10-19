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
            string hotdll = Path.GetFullPath("../../../Assets/Out/HotFixDll.dll.bytes");

            string bindroot = Path.GetFullPath("../../../ILRuntimeGen");
            string bindgen = Path.Combine(bindroot, "src");

            ILRuntimeCLRBinding.GenerateCLRBindingByAnalysis(hotdll, bindgen);

            AddVSProject(bindroot);

            MakeGenCode(Path.Combine(bindroot, "make.bat"));

        }

        private static void MakeGenCode(string bat)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = bat;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
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
