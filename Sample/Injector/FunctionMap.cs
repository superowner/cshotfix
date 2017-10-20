using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using static HotFixInjector.Program;

namespace HotFixInjector
{
    public class FuncData
    {
        public string returnType;
        public List<string> paramsType = new List<string>();
        public string funcName;
    }
    /// <summary>
    /// 用于序列化函数和注入关系
    /// </summary>
    public class FunctionMap
    {
        public static List<FuncData> m_FunctionMap = new List<FuncData>();
        private static string m_fileName  =Path.Combine( Path.GetDirectoryName( System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) , "FunctionMap.txt");
        public static void WriteData()
        {
            StreamWriter sw = new StreamWriter(m_fileName);
            sw.WriteLine(m_FunctionMap.Count);
            foreach (var f in m_FunctionMap)
            {
                sw.WriteLine(f.returnType);
                sw.WriteLine(f.paramsType.Count);
                foreach (var p in f.paramsType)
                {
                    sw.WriteLine(p);
                }
                sw.WriteLine(f.funcName);
            }
            sw.Flush();
            sw.Close();
        }
        public static void ReadData()
        {
            StreamReader sr = new StreamReader(m_fileName);
            int count = int.Parse(sr.ReadLine());
            for(int i=0;i<count;++i)
            {
                FuncData data = new FuncData();
                data.returnType = sr.ReadLine();
                int paramcount = int.Parse( sr.ReadLine());
                for(int n = 0; n<paramcount;n++)
                {
                    data.paramsType.Add(sr.ReadLine());
                }
                data.funcName = sr.ReadLine();
                m_FunctionMap.Add(data);
            }
            sr.Close();
        }

        public static string AddFuncData(MethodInfoData mid)
        {
            FuncData data = new FuncData();
            if(mid.methodinfo is MethodInfo)
            {
                data.returnType = (mid.methodinfo as MethodInfo).ReturnParameter.ParameterType.Name;
            }
            else
            {
                data.returnType = "";
            }
            ParameterInfo[] _params = mid.methodinfo.GetParameters();
            foreach(var pi in _params)
            {
                data.paramsType.Add(pi.ParameterType.Name);
            }
            data.funcName = "hotfix_func" + m_FunctionMap.Count.ToString();

            m_FunctionMap.Add(data);
            return data.funcName;
        }
        public static FieldDefinition GetFunctionField(TypeDefinition functionDef, MethodDefinition method)
        {
            FuncData returnData = GetFuncData(functionDef, method);
            string name = returnData.funcName+"_delegate";
            foreach(var f in functionDef.Fields)
            {
                if( f.Name == name )
                {
                    return f;
                }
            }
            return null;
        }

        private static FuncData GetFuncData(TypeDefinition functionDef, MethodDefinition method)
        {
            string returnType = "void";
            if (method.IsConstructor)
            {
                returnType = "";
            }
            else
            {
                if(method.ReturnType.Name.ToLower() == "void")
                {
                    returnType = "void";
                }
                else
                {
                    returnType = method.ReturnType.Name;
                }
            }
            List<string> paramsType = new List<string>();
            var parameters = method.Parameters;
            foreach(var p in parameters)
            {
                paramsType.Add(p.ParameterType.Name);
            }

            FuncData returnData = m_FunctionMap.Find((FuncData data) =>
            {
                if(data.returnType.ToLower() != returnType.ToLower())
                {
                    return false;
                }
                if(paramsType.Count != data.paramsType.Count)
                {
                    return false;
                }
                else
                {
                    int count = paramsType.Count;
                    for(int i =0;i<count;++i)
                    {
                        if(paramsType[i].ToLower() != data.paramsType[i].ToLower())
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
            return returnData;
        }

        public static MethodDefinition GetFunctionMethod(TypeDefinition functionDef, MethodDefinition method)
        {
            FuncData returnData = GetFuncData(functionDef, method);
            string name = returnData.funcName;
            foreach (var f in functionDef.Methods)
            {
                if (f.Name == name)
                {
                    return f;
                }
            }
            return null;
        }
    }
}
