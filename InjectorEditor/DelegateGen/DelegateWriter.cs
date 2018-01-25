using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public class DelegateWriter
{
    public void WriteFunctionDelegate(string filePath, List<LMethodInfo> lines)
    {
        string fileHeader =
"using System;\r\n" +
"using System.Collections.Generic;\r\n" +
"using System.Linq;\r\n" +
"using System.Text;\r\n" +
"public class FunctionDelegate\r\n" +
"{\r\n";
        int funcIndex = 0;
        string fileFunctions = "";
        foreach (var info in lines)
        {
            string funcline = "     public delegate " + info.m_ReturnString + " ";

            funcline += "method_delegate" + funcIndex++ + "(";
            int paramIndex = 0;
            foreach (var param in info.m_Params)
            {
                string paramType = "";
                if (param.m_RefOut == RefOutArrayEnum.Ref)
                {
                    paramType += "ref ";
                }
                else if (param.m_RefOut == RefOutArrayEnum.Out)
                {
                    paramType += "out ";
                }
                else
                {

                }

                paramType += param.m_ParamString;
                paramType += " arg" + paramIndex++;
                if (paramIndex < info.m_Params.Count)
                {
                    paramType += ",";
                }
                funcline += paramType;
            }
            funcline += ");\r\n";
            fileFunctions += funcline;
        }
        string fileEnd =
"}";

        string outputString = fileHeader + fileFunctions + fileEnd;

        using (FileStream file = new FileStream(filePath + "/FunctionDelegate.cs", FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.Write(outputString);
                sw.Flush();
                sw.Close();
                file.Close();
            }
        }

    }
}

