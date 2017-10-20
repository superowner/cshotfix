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

            string bindgen = Path.GetFullPath("../../../Assets/ilrt_gen");
            ILRuntimeCLRBinding.GenerateCLRBindingByAnalysis(hotdll, bindgen);
        }

    }
}
