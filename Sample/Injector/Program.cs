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
using System.Reflection;
using System.Text;
using LCL;

namespace HotFixInjector
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args == null)
            {
                Console.WriteLine("args error");
                return;
            }
            string dll = Path.GetFullPath(args[0]);
            string genpath = Path.GetFullPath(args[1]);

            Assembly dllcode = Assembly.LoadFrom(dll);
            if(dllcode == null)
            {
                Console.WriteLine("dll not find path:"+dll);
                return;
            }
            Assembly gencode = Assembly.LoadFrom(genpath);
            if (gencode == null)
            {
                Console.WriteLine("dll not find path:" + dll);
                return;
            }

            //先生成gen
            GenCode(dllcode, gencode);
            
        }

        //找到需要注入的方法，并且生成对应的绑定函数
        private static void GenCode(Assembly dllcode, Assembly gencode)
        {
            List<MethodInfo> methodList = new List<MethodInfo>();

            //找到需要注入的方法
            Module[] modules = dllcode.GetModules();
            foreach(var module in modules)
            {
                foreach(var type in module.GetTypes())
                {
                    //过滤掉不支持的类型
                    if(!IsNeedInjectType(type))
                    {
                        continue;
                    }
                    foreach(var method in type.GetMethods())
                    {
                        if(!IsNeedInjectMethod(method))
                        {
                            continue;
                        }
                        methodList.Add(method);
                    }        
                }
            }

            //生成绑定代码
            //Module[] genmodules = gencode.GetModules();
            //foreach (var module in genmodules)
            //{
            //    foreach (var type in module.GetTypes())
            //    {
            //        foreach(var member in type.GetMembers())
            //        {
            //            if (member.DeclaringType == typeof(Object) ||
            //                member.MemberType == MemberTypes.Constructor)
            //            {

            //            }
            //        }
            //    }
            //}

            List<MemberInfo> delegateList = new List<MemberInfo>();
            //识别需要Inject的方法有哪些类型
            foreach(var method in methodList)
            {

            }


        }

        private static bool IsNeedInjectType(Type type)
        {
            if(type == null)
            {
                return false;
            }
            if (type.Name.Contains("<") || type.IsInterface || type.GetMethods().Length == 0) // skip anonymous type and interface
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool IsNeedInjectMethod(MethodInfo method)
        {
            if(method == null)
            {
                return false;
            }

            var attr = method.GetCustomAttributes(typeof(InjectAttribute), true);
            if(attr != null && attr.Length > 0)
            {
                InjectAttribute flag = attr[0] as InjectAttribute;
                if(flag.Flag == InjectFlag.Inject)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
