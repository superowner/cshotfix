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
using Mono.Cecil.Cil;
using System.Reflection.Emit;
using Mono.Cecil;

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
            //        if (type.BaseType == typeof(Delegate) || type.BaseType == typeof(MulticastDelegate))
            //        {
            //            MethodInfo info = type.GetMethod("Invoke");
            //            ParameterInfo[] _paraminfos = info.GetParameters();
            //            ParameterInfo _returnpi = info.ReturnParameter;
            //        }
            //    }
            //}

            List<MethodData> delegateList = new List<MethodData>();
            //识别需要Inject的方法有哪些类型
            foreach(var method in methodList)
            {
                ParameterInfo _hr = method.ReturnParameter;
                ParameterInfo[] _params = method.GetParameters();

                MemberInfo _find = null;

                if(FindDelegate(_params, _hr, delegateList, out _find))
                {
                    //如果已经找到了的话，就不用再添加了
                }
                else
                {

                }
            }


        }
        struct MethodData
        {
            ParameterInfo[] param;
            ParameterInfo hr;
        }
        private static bool FindDelegate(ParameterInfo[] _params, ParameterInfo _hr, List<Type> delegateList, out MemberInfo info)
        {
            info = delegateList.Find((Type t) =>
            {
                MethodInfo _info = t.GetMethod("Invoke");
                ParameterInfo t_hr = _info.ReturnParameter;
                ParameterInfo[] t_params = _info.GetParameters();
                if (t_params == null || t_params.Length < 1)
                {
                    Console.WriteLine("delegate param null or less 1" + _info.Name);
                    return false;
                }
                else
                {
                    if (t_params[0].ParameterType != typeof(Object))
                    {
                        return false;
                    }
                }
                if (_params != null)
                {

                    //委托会默认把被注入的类作为第一个参数的，所以它比被注入的方法多一个参数
                    if (_params.Length != t_params.Length - 1)
                    {
                        return false;
                    }

                    for (int i = 0; i < _params.Length; ++i)
                    {
                        ParameterInfo tar_info = _params[i];
                        ParameterInfo find_info = t_params[i + 1];
                        if (tar_info.ParameterType != find_info.ParameterType)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (t_params.Length > 1)
                    {
                        return false;
                    }
                }
                if (t_hr != null)
                {
                    if (_hr == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (t_hr.ParameterType == _hr.ParameterType)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (_hr != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            });
            return info != null;
        }
        private static void AddDelegates(List<MethodData> list, Assembly genCode)
        {
            //注入delegate
            //http://www.cnblogs.com/xuanhun/archive/2012/06/03/2532922.html

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
