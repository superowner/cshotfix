using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


public class DelegateGen
{
    private Assembly m_Assembly = null;

    private DelegateWriter m_DelegateWriter = null;
    public void Run(string dllPath, string delegatePath)
    {
        var lines = LoadAssembly(dllPath);

        m_DelegateWriter = new DelegateWriter();

        m_DelegateWriter.WriteFunctionDelegate(delegatePath, lines);
    }
    private List<LMethodInfo> LoadAssembly(string assemblyName)
    {
        List<LMethodInfo> funcLines = new List<LMethodInfo>();

        m_Assembly = Assembly.LoadFile(assemblyName);
        var types = m_Assembly.GetTypes();
        int methodId = 0;
        foreach (var type in types)
        {
            if (type.Name.Contains("FunctionDelegate"))
            {
                continue;
            }
            if (type.DeclaringType != null && type.DeclaringType.Name.Contains("FunctionDelegate"))
            {
                continue;
            }
            var methodInfos = type.GetMethods();
            foreach (var methodinfo in methodInfos)
            {
                //只处理本类的方法，派生方法不要
                if (methodinfo.DeclaringType.FullName != type.FullName)
                {
                    continue;
                }
                LMethodInfo info = new LMethodInfo();
                var returnparamter = methodinfo.ReturnParameter;
                info.m_ReturnString = returnparamter.ParameterType.ToString();
                if (info.m_ReturnString.Contains("Void"))
                {
                    info.m_ReturnString = "void";
                }

                var paramters = methodinfo.GetParameters();
                if (paramters != null)
                {
                    foreach (var pi in paramters)
                    {
                        ParamData paramdata = new ParamData();
                        if (pi.IsOut)
                        {
                            paramdata.m_RefOut = RefOutArrayEnum.Out;
                        }
                        else if (pi.ParameterType.IsByRef)
                        {
                            paramdata.m_RefOut = RefOutArrayEnum.Ref;
                        }


                        paramdata.m_ParamString = pi.ParameterType.ToString().Replace("&", "");
                        info.m_Params.Add(paramdata);
                    }
                }
                funcLines.Add(info);

            }
        }

        return funcLines;
    }
}

