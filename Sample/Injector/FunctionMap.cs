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
        public List<string> paramsType;
        public string funcName;
    }
    /// <summary>
    /// 用于序列化函数和注入关系
    /// </summary>
    public class FunctionMap
    {
        public static List<FuncData> m_FunctionMap = new List<FuncData>();

        public static void WriteData()
        {
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".txt";
            StreamWriter sw = new StreamWriter(path);
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
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".txt";
            StreamReader sr = new StreamReader(path);
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
            string returnType = method.ReturnType.DeclaringType.Name;
            List<string> paramsType = new List<string>();
            var parameters = method.Parameters;
            foreach(var p in parameters)
            {
                paramsType.Add(p.ParameterType.Name);
            }

            FuncData returnData = m_FunctionMap.Find((FuncData data) =>
            {
                if(data.returnType != returnType)
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
                        if(paramsType[i] != data.paramsType[i])
                        {
                            return false;
                        }
                    }
                }
                return true;
            });

            string name = returnData.funcName;
            foreach(var f in functionDef.Fields)
            {
                if( f.Name == name )
                {
                    return f;
                }
            }
            return null;
        }
    }
}
