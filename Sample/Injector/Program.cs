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
using LCL;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System.Reflection;

namespace HotFixInjector
{

    public enum FindFlag
    {
        Error = -1,
        Find = 0,
        NotFind = 1
    }
    class MethodData
    {
        public ParameterInfo[] param;
        public ParameterInfo hr;
        public string name;
        public MethodInfoData methodInfo;
    }

    public class MethodInfoData
    {
        public MethodBase methodinfo;
        //委托类型名字
        public string delegatename;
        //方法对应的函数变量
        public string functionname;
        public MethodInfoData(MethodBase info, string name)
        {
            methodinfo = info;
            delegatename = name;
        }
    }
    class Program
    {
        private static TypeReference m_InjectRef = null;
        private static TypeDefinition m_FuncDef = null;


        static void Main(string[] args)
        {
            bool testinject = false;

            if (testinject || (args != null && args[0] == "inject"))
            {
                FunctionMap.ReadData();
                InjectAssembly(Path.GetFullPath("../../../Library/ScriptAssemblies/Assembly-CSharp.dll"));
            }
            else
            {
                string genpath = Path.GetFullPath("../../../Assets/inject_gen");

                //这里貌似只能使用工程中引用的程序集，不能使用外部的dll，原因不明白，知道的可以帮忙解释下
                Assembly dllcode = Assembly.Load("Assembly-CSharp");
                if (dllcode == null)
                {
                    Console.WriteLine("dll not find");
                    return;
                }

                List<MethodInfoData> needfixMethods = new List<MethodInfoData>();
                CollectMethods(dllcode, needfixMethods);
                //先生成委托gen
                GenDelegateCode(genpath, needfixMethods);
                //生成函数变量
                GenFunctionVar(genpath, needfixMethods);

                FunctionMap.WriteData();
            }
        }
        private static AssemblyDefinition m_Assembly = null;
        public static void InjectAssembly(string assembly_path)
        {
            var readerParameters = new ReaderParameters { ReadSymbols = false };
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(assembly_path, readerParameters);
            m_Assembly = assembly;
            m_InjectRef = assembly.MainModule.ImportReference(typeof(InjectAttribute));
            //if (assembly.Modules.Any(module => module.Types.Any(x => x.Namespace == "__CSHotFix" && x.Name == "INJECTED")))
            //{
            //    Console.WriteLine("This Assembly is already injected!");
            //    return;
            //}

            foreach (var module in assembly.Modules)
            {
                foreach (var typ in module.Types)
                {
                    if (typ.FullName == typeof(HotFix.HotFixFunction).FullName)
                    {
                        m_FuncDef = typ;
                        break;
                    }
                }
            }


            foreach (var module in assembly.Modules)
            {
                foreach (var typ in module.Types)
                {
                    if (IsNeedInjectType(typ))
                    {
                        foreach (var method in typ.Methods)
                        {
                            if (IsNeedInjectMethod(method))
                            {
                                InjectMethod(typ, method);
                            }
                        }
                    }
                }
            }


            //var objType = assembly.MainModule.ImportReference(typeof(object));
            //assembly.MainModule.Types.Add(new TypeDefinition("__CSHotFix", "INJECTED", Mono.Cecil.TypeAttributes.Class, objType));

            var writerParameters = new WriterParameters { WriteSymbols = true };
            assembly.Write(assembly_path, writerParameters);

            Console.WriteLine("Inject Success!!!");

            if (assembly.MainModule.SymbolReader != null)
            {
                assembly.MainModule.SymbolReader.Dispose();
            }
        }
        private static FieldReference GetGeneric(FieldDefinition definition)
        {
            if (definition.DeclaringType.HasGenericParameters)
            {
                var declaringType = new GenericInstanceType(definition.DeclaringType);
                foreach (var parameter in definition.DeclaringType.GenericParameters)
                {
                    declaringType.GenericArguments.Add(parameter);
                }
                return new FieldReference(definition.Name, definition.FieldType, declaringType);
            }

            return definition;
        }

        private static TypeReference GetGeneric(TypeDefinition definition)
        {
            if (definition.HasGenericParameters)
            {
                var genericInstanceType = new GenericInstanceType(definition);
                foreach (var parameter in definition.GenericParameters)
                {
                    genericInstanceType.GenericArguments.Add(parameter);
                }
                return genericInstanceType;
            }

            return definition;
        }
        private static MethodReference MakeGenericMethod(MethodReference self, params TypeReference[] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException();

            var instance = new GenericInstanceMethod(self);
            foreach (var argument in arguments)
                instance.GenericArguments.Add(argument);

            return instance;
        }

        public static void InjectMethod(TypeDefinition type, MethodDefinition method)
        {
            if (type.Name.Contains("<") || type.IsInterface || type.Methods.Count == 0) // skip anonymous type and interface
                return;
            if (method.Name == ".cctor")
                return;
            if(!method.HasBody)
            {
                return;
            }
            FieldDefinition fielditem = FunctionMap.GetFunctionField(m_FuncDef, method);
            MethodDefinition funcitem = FunctionMap.GetFunctionMethod(m_FuncDef, method);
            

            ////记住方法原来的开始插入点
            var insertPoint = method.Body.Instructions[0];
            var ilGenerator = method.Body.GetILProcessor();

            //插入注入代码
            ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldsfld, fielditem));
            //如果为false还原插入点
            ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Brfalse, insertPoint));


            //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldsfld, fielditem));
            //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Dup));
            //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldfld, parameter));
            //ilGenerator.InsertBefore(insertPoint, CreateLoadIntConst(ilGenerator, 0));
            //if (method.IsStatic)
            //{
            //    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldnull));
            //}
            //else
            //{
            //    ilGenerator.InsertBefore(insertPoint, CreateLoadArg(ilGenerator, 0));
            //}
            //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Stelem_Ref));

            //for (int i = 0; i < method.Parameters.Count; i++)
            //{
            //    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Dup));
            //    //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldsfld, item));
            //    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldfld, parameter));

            //    int index = (i + (method.IsStatic ? 0 : 1));
            //    ilGenerator.InsertBefore(insertPoint, CreateLoadIntConst(ilGenerator, i + 1));
            //    ilGenerator.InsertBefore(insertPoint, CreateLoadArg(ilGenerator, index));

            //    if (method.Parameters[i].ParameterType.IsValueType)
            //    {
            //        ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Box, method.Parameters[i].ParameterType));
            //    }

            //    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Stelem_Ref));
            //}

            ////ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldsfld, item));
            //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Call, invokeDeclare));

            //if (method.ReturnType.Name == "Void")
            //    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Pop));
            //else if (method.ReturnType.IsValueType)
            //{
            //    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Unbox_Any, method.ReturnType));
            //}
            //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ret));

        }


        private static Instruction CreateLoadIntConst(ILProcessor ilGenerator, int c)
        {
            switch (c)
            {
                case 0:
                    return ilGenerator.Create(OpCodes.Ldc_I4_0);
                case 1:
                    return ilGenerator.Create(OpCodes.Ldc_I4_1);
                case 2:
                    return ilGenerator.Create(OpCodes.Ldc_I4_2);
                case 3:
                    return ilGenerator.Create(OpCodes.Ldc_I4_3);
                case 4:
                    return ilGenerator.Create(OpCodes.Ldc_I4_4);
                case 5:
                    return ilGenerator.Create(OpCodes.Ldc_I4_5);
                case 6:
                    return ilGenerator.Create(OpCodes.Ldc_I4_6);
                case 7:
                    return ilGenerator.Create(OpCodes.Ldc_I4_7);
                case 8:
                    return ilGenerator.Create(OpCodes.Ldc_I4_8);
                case -1:
                    return ilGenerator.Create(OpCodes.Ldc_I4_M1);
            }
            if (c >= sbyte.MinValue && c <= sbyte.MaxValue)
                return ilGenerator.Create(OpCodes.Ldc_I4_S, (sbyte)c);

            return ilGenerator.Create(OpCodes.Ldc_I4, c);
        }
        private static Instruction CreateLoadArg(ILProcessor ilGenerator, int c)
        {
            switch (c)
            {
                case 0:
                    return ilGenerator.Create(OpCodes.Ldarg_0);
                case 1:
                    return ilGenerator.Create(OpCodes.Ldarg_1);
                case 2:
                    return ilGenerator.Create(OpCodes.Ldarg_2);
                case 3:
                    return ilGenerator.Create(OpCodes.Ldarg_3);
            }
            if (c > 0 && c < byte.MaxValue)
                return ilGenerator.Create(OpCodes.Ldarg_S, (byte)c);

            return ilGenerator.Create(OpCodes.Ldarg, c);
        }

        //找到需要注入的方法
        private static void CollectMethods(Assembly dllcode, List<MethodInfoData> methodList)
        {
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
                    //处理类的构造函数
                    foreach(var ctr in type.GetConstructors())
                    {
                        if (!IsNeedInjectMethod(ctr))
                        {
                            continue;
                        }
                        methodList.Add(new MethodInfoData(ctr, ""));
                    }
                    //处理方法
                    foreach(var method in type.GetMethods())
                    {
                        //跳过基类的方法
                        if(method.DeclaringType != null &&  method.DeclaringType.Name != type.Name)
                        {
                            continue;
                        }
                        if(!IsNeedInjectMethod(method))
                        {
                            continue;
                        }
                        methodList.Add(new MethodInfoData(method, ""));
                    }        
                }
            }
        }
        private static void GenDelegateCode(string genPath, List<MethodInfoData> methodList)
        {
            List<MethodData> delegateList = new List<MethodData>();
            int index = 0;
            //识别需要Inject的方法有哪些类型
            foreach(var method in methodList)
            {
                ParameterInfo _hr = null;
                if (method.methodinfo is MethodInfo)
                {
                    //构造函数没有返回值，这里只有方法可能有
                    _hr = (method.methodinfo as MethodInfo).ReturnParameter;
                }
                ParameterInfo[] _params = method.methodinfo.GetParameters();

                MethodData _find = null;
                FindFlag flag = FindDelegate(_params, _hr, delegateList, out _find);
                if (flag == FindFlag.NotFind)
                {
                    _find = new MethodData() { param = _params, hr = _hr, methodInfo = method };
                    delegateList.Add(_find);

                    //设置委托名字
                    _find.methodInfo.delegatename = (_find.name == "" || _find.name == null) ? "delegate_" + index.ToString() : _find.name;
                }
                else if(flag == FindFlag.Find)
                {
                    //直接设置委托名字
                    method.delegatename = _find.methodInfo.delegatename;
                }
                index++;
            }
            AddDelegates(delegateList, genPath);
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
                    if(t_params == null || _params.Length != t_params.Length)
                    {
                        continue;
                    }
                    for (int i = 0; i < _params.Length; ++i)
                    {
                        ParameterInfo tar_info = _params[i];
                        ParameterInfo find_info = t_params[i];
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
                    if(t_params != null)
                    {
                        continue;
                    }
                }
                find = FindFlag.Find;
            }
            return find;
        }
        private static void GenFunctionVar( string genFilePath, List<MethodInfoData> list)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }
            //写delegate文件
            StreamWriter sw = new StreamWriter(Path.Combine(genFilePath, "HotFixFunction.cs"));
            sw.WriteLine("//======================================================================" + Environment.NewLine +
            "//                                                                       " + Environment.NewLine +
            "//        created by lichunlin                                           " + Environment.NewLine +
            "//        qq:576067421                                                   " + Environment.NewLine +
            "//        git:https://github.com/lichunlincn/cshotfix                    " + Environment.NewLine +
            "//                                                                       " + Environment.NewLine +
            "//====================================================================== " + Environment.NewLine +
            "using System.Collections; " + Environment.NewLine +
             "using System; " + Environment.NewLine +
             "using HotFix.HotFixDelegate; " + Environment.NewLine +
             "namespace HotFix" + Environment.NewLine +
             "{" + Environment.NewLine +
             "       public class HotFixFunction" + Environment.NewLine +
             "       {");
            int count = list.Count;
            //检测是否有相同名称的，有的话就用序号表示
            for (int i = 0; i < count; ++i)
            {
                MethodInfoData mid = list[i];
                string name = FunctionMap.AddFuncData(mid);

                //求出方法和对应的委托函数变量的对应关系，注入到mono代码需要
                mid.functionname = name;

                //输出一行
                //public static DelegateImp0 __hotfix__GameMono__GameLoop__HelloWorld0;
                string linestr = "          public static " + mid.delegatename + " " + name+"_delegate" + ";";
                sw.WriteLine(linestr);
                ParameterInfo _hr = null;
                if(mid.methodinfo is MethodInfo)
                {
                    _hr = (mid.methodinfo as MethodInfo).ReturnParameter;
                }
                string returnstr = _hr == null ? " void " : GetParameterTypeName(_hr);
                ParameterInfo[] _params = mid.methodinfo.GetParameters();
                string params_string = "object _this";
                string params_string_no_type = "_this";
                
                if (_params != null && _params.Length > 0)
                {
                    int pcount = _params.Length;
                    for (int pi = 0; pi < pcount; ++pi)
                    {
                        ParameterInfo info = _params[pi];
                        string ref_str = info.IsOut ? "" : (info.ParameterType.IsByRef ? " ref " : "");
                        string out_str = info.IsOut ? " out " : "";
                        string param_type_string = GetParameterTypeName(info);
                        string param_name = " _param" + pi.ToString() + "_" + GetParamterNickName(info);
                        params_string += "," + ref_str + out_str + param_type_string + param_name;
                        params_string_no_type += ", " + ref_str + out_str + param_name;
                    }
                }

               
                string funcstr = "          public static " + returnstr+ " "+ mid.functionname + "("+ params_string + ")" + Environment.NewLine +
                    "         {" + Environment.NewLine +
                    "               "+ ((_hr == null|| _hr.ParameterType.Name.ToLower() =="void")?" ":"return " )+  mid.functionname + "_delegate" + "("+params_string_no_type+");"+Environment.NewLine+
                    "         }";
                sw.WriteLine(funcstr);
            }
            sw.WriteLine("       }" + Environment.NewLine +
             "}");
            sw.Flush();
            sw.Close();
        }
        private static void AddDelegates(List<MethodData> list, string genFilePath)
        {
            if(list == null || list.Count ==0)
            {
                return;
            }
            //写delegate文件
            StreamWriter sw = new StreamWriter(Path.Combine( genFilePath , "HotFixDelegate.cs"));
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

            foreach(MethodData md in list)
            {
                ParameterInfo _hr = md.hr;
                string hr_string = _hr == null ? "void" : GetParameterTypeName(_hr);
                ParameterInfo[] _params = md.param;
                string params_string = "object _this";
                if(_params != null && _params.Length > 0)
                {
                    int count = _params.Length;
                    for(int i =0;i<count;++i)
                    {
                        ParameterInfo info = _params[i];
                        string ref_str = info.IsOut ?"":(info.ParameterType.IsByRef ? " ref " : "");
                        string out_str = info.IsOut ? " out " : "";
                        string param_type_string = GetParameterTypeName(info);
                        params_string += "," + ref_str + out_str + param_type_string + " _param"+ i.ToString()+"_"+ GetParamterNickName(info);
                    }
                }

                string line_string = "           public delegate " + hr_string + " " + md.methodInfo.delegatename + "(" + params_string + ");";
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
        private static string GetParameterTypeName(ParameterInfo info)
        {
            string param_type_string = "object";
            if(info.ParameterType.IsPrimitive)
            {
                param_type_string = info.ParameterType.Name;
            }
            else if(info.ParameterType.IsClass)
            {
                param_type_string = "object";
                if(info.ParameterType.Name.Contains("&"))
                {
                    string shortname = info.ParameterType.FullName.Replace("&", "");
                    Type type = Type.GetType(shortname);
                    if(type != null)
                    {
                        if(type.IsPrimitive || IsOtherPrimitive(shortname))
                        {
                            param_type_string = shortname;
                        }
                        else
                        {
                            //支持自定义类
                            param_type_string = shortname;
                        }
                    }
                    else
                    {
                         //不是系统的基础类型，查看是否是我们关心的基础类型
                        if(IsOtherPrimitive(shortname))
                        {
                            param_type_string = shortname;
                        }
                        else
                        {
                            //支持自定义类
                            param_type_string = shortname;
                        }   
                    }


                }
                else
                {
                    if (info.ParameterType == typeof(String))
                    {
                        param_type_string = "String";
                    }
                }


            }
            else
            {
                param_type_string = info.ParameterType.FullName;
            }
            if(param_type_string == "Void" || param_type_string == "System.Void")
            {
                param_type_string = "void";
            }
            return param_type_string;
        }

        private static bool IsOtherPrimitive(string name)
        {
            name = name.ToLower();
            if(name.Contains("unityengine.vector2")||
               name.Contains("unityengine.vector3") ||
               name.Contains("unityengine.quaternion") ||
               name.Contains("unityengine.vector4") ||
               name.Contains("unityengine.ray") ||
               name.Contains("system.string"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsNeedInjectType(TypeDefinition type)
        {
            if (type == null)
            {
                return false;
            }
            if (type.Namespace == null || type.Name.Contains("<") || type.IsInterface || type.Methods.Count == 0) // skip anonymous type and interface
            {
                return false;
            }
            else if (!type.Namespace.Contains("LCL.Logic"))
            {
                //这里假设只有Mono的逻辑代码会出错
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool IsNeedInjectType(Type type)
        {
            if(type == null)
            {
                return false;
            }
            if (type.Namespace == null || type.Name.Contains("<") || type.IsInterface || type.GetMethods().Length == 0) // skip anonymous type and interface
            {
                return false;
            }
            else if(!type.Namespace.Contains("LCL.Logic"))
            {
                //这里假设只有Mono的逻辑代码会出错
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool IsNeedInjectMethod(MethodDefinition method)
        {
            if (method == null)
            {
                return false;
            }
            var attrs = method.CustomAttributes;//.FirstOrDefault(ca => ca.AttributeType == m_InjectRef);
            foreach(var attr in attrs)
            {
                if( attr.AttributeType.FullName == m_InjectRef.FullName)
                {
                    InjectFlag hotfixType = (InjectFlag)attr.ConstructorArguments[0].Value;
                    return hotfixType == InjectFlag.Inject;
                } 
            }
            return true;
            
        }
        private static bool IsNeedInjectMethod(MethodBase method)
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
