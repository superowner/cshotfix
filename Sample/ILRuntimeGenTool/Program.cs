using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILRuntimeGenTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            if(args == null)
            {
                Console.WriteLine("请输入需要绑定的dll 和 生成代码的路径");
                return;
            }
            if(args.Length == 2)
            {
                string hotdll = args[0];
                string bindgen = args[1];
                ILRuntimeCLRBinding.GenerateCLRBindingByAnalysis(hotdll, bindgen);
                Console.WriteLine("bind ok, 请编译ILRuntimeGen工程");
            }
            else
            {
                Console.WriteLine("参数输入错误");
                return;
            }
        }
    }
}
