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
            string genbat = Path.GetFullPath(args[2]);

            Assembly dllcode = Assembly.LoadFrom(dll);
            if(dllcode == null)
            {
                Console.WriteLine("dll not find path:"+dll);
                return;
            }


            //先生成gen
            GenCode(dllcode, genpath);

            //编译gen
            MakeGenCode(genbat);

        }

        //找到需要注入的方法，并且生成对应的绑定函数
        private static void GenCode(Assembly dllcode, string  genPath)
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

            List<MethodData> delegateList = new List<MethodData>();
            //识别需要Inject的方法有哪些类型
            foreach(var method in methodList)
            {
                ParameterInfo _hr = method.ReturnParameter;
                ParameterInfo[] _params = method.GetParameters();

                MethodData _find = null;
                FindFlag flag = FindDelegate(_params, _hr, delegateList, out _find);
                if(flag == FindFlag.NotFind)
                {
                    _find = new MethodData() {param = _params, hr = _hr };
                    delegateList.Add(_find);
                }
            }

            AddDelegates(delegateList, genPath);
        }
        public enum FindFlag
        {
            Error =-1,
            Find =0,
            NotFind =1
        }
        class MethodData
        {
            public ParameterInfo[] param;
            public ParameterInfo hr;
            public string name;
        }
        private static FindFlag FindDelegate(ParameterInfo[] _params, ParameterInfo _hr, List<MethodData> delegateList, out MethodData info)
        {
            FindFlag find = FindFlag.NotFind;
            info = null;
            for(int n =0; n< delegateList.Count; ++n)
            {
                find = FindFlag.NotFind;
                info = delegateList[n];
                ParameterInfo t_hr = info.hr;
                ParameterInfo[] t_params = info.param;
                if (t_params == null || t_params.Length < 1)
                {
                    Console.WriteLine("delegate param null or less 1");
                    return FindFlag.Error;
                }
                else
                {
                    if (t_params[0].ParameterType != typeof(object))
                    {
                        Console.WriteLine("委托的第一个参数不是object类型");
                        return FindFlag.Error;
                    }
                }
                //先比较返回值情况
                if (t_hr != null)
                {
                    if (_hr == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (t_hr.ParameterType == _hr.ParameterType)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (_hr != null)
                    {
                        continue;
                    }
                    else
                    {

                    }
                }
                //比较参数
                if (_params != null)
                {
                    //委托会默认把被注入的类作为第一个参数的，所以它比被注入的方法多一个参数
                    if (_params.Length != t_params.Length - 1)
                    {
                        continue;
                    }
                    for (int i = 0; i < _params.Length; ++i)
                    {
                        ParameterInfo tar_info = _params[i];
                        ParameterInfo find_info = t_params[i + 1];
                        if(tar_info.ParameterType.IsSubclassOf(typeof(object)) && find_info.ParameterType.IsSubclassOf(typeof(object)))
                        {

                        }
                        else if (tar_info.ParameterType != find_info.ParameterType)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (t_params.Length > 1)
                    {
                        continue;
                    }
                }
                find = FindFlag.Find;
            }
            return find;
        }
        private static void AddDelegates(List<MethodData> list, string genFilePath)
        {
            if(list == null || list.Count ==0)
            {
                return;
            }
            //写delegate文件
            StreamWriter sw = new StreamWriter(genFilePath);
            sw.WriteLine("//======================================================================"+Environment.NewLine+
                        "//                                                                       "+Environment.NewLine+
                        "//        created by lichunlin                                           "+Environment.NewLine+
                        "//        qq:576067421                                                   "+Environment.NewLine+
                        "//        git:https://github.com/lichunlincn/cshotfix                    "+Environment.NewLine+
                        "//                                                                       "+Environment.NewLine+
                        "//====================================================================== "+Environment.NewLine+
                        "using System.Collections; " + Environment.NewLine +
                         "using System; " + Environment.NewLine+
                         "namespace HotFix" + Environment.NewLine+
                         "{"+Environment.NewLine+
                         "       namespace HotFixDelegate"+Environment.NewLine+
                         "       {");

            int index = 0;
            foreach(MethodData md in list)
            {
                string name = (md.name == "" || md.name == null)? "delegate_" + index.ToString() : md.name;
                index++;
                ParameterInfo _hr = md.hr;
                string hr_string = _hr == null ? "void" : GetParameterName(_hr);
                ParameterInfo[] _params = md.param;
                string params_string = "object _this";
                if(_params != null && _params.Length > 0)
                {
                    int count = _params.Length;
                    for(int i =1;i<count;++i)
                    {
                        ParameterInfo info = _params[i];
                        string ref_str = info.IsRetval ? " ref " : "";
                        string out_str = info.IsOut ? " out " : "";
                        string param_type_string = GetParameterName(info);
                        params_string += "," + ref_str + out_str + param_type_string + " _param"+ i.ToString()+"_"+ GetParamterNickName(info);
                    }
                }

                string line_string = "           public delegate " + hr_string + " " + name + "(" + params_string + ");";
                sw.WriteLine(line_string);
            }

            sw.WriteLine("       }"+ Environment.NewLine+
                         "}");
            sw.Flush();
            sw.Close();
        }
        private static string GetParamterNickName(ParameterInfo info)
        {
            return info.ParameterType.Name.Replace("[", "").Replace("]", "").Replace("`", "").Replace("&","");
        }
        private static string GetParameterName(ParameterInfo info)
        {
            string param_type_string = "object";
            if (!info.ParameterType.IsSubclassOf(typeof(object)))
            {
                param_type_string = info.ParameterType.Name;
            }
            return param_type_string;
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

        private static void MakeGenCode(string bat)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = bat;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }
    }
}
