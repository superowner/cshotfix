/*
 * LCL support c# hotfix here.
 * Copyright (C) LCL. All rights reserved.
 * URL:https://github.com/qq576067421/cshotfix
 * QQ:576067421
 * QQ Group:673735733
 * Licensed under the GNU License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://fsf.org/
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LCL
{
    public class DelegateWriter
    {
        public void WriteFunctionDelegate(string filePath, List<LMethodInfo> lines)
        {
            string fileHeader =
    "/*\r\n" +
    "* LCL support c# hotfix here.\r\n" +
    "*Copyright(C) LCL.All rights reserved.\r\n" +
    "* URL:https://github.com/qq576067421/cshotfix \r\n" +
    "*QQ:576067421 \r\n" +
    "* QQ Group: 673735733 \r\n" +
    " * Licensed under the GNU License (the \"License\"); you may not use this file except in compliance with the License. You may obtain a copy of the License at \r\n" +
    "* http://fsf.org/ \r\n" +
    "* Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an \"AS IS\" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License. \r\n" +
    "*/  \r\n" +
    "using System;\r\n" +
    "using System.Collections.Generic;\r\n" +
    "using System.Linq;\r\n" +
    "using System.Text;\r\n" +
    "public class LCLFunctionDelegate\r\n" +
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
            FileStream file = null;
            StreamWriter sw = null;
            //有什么错误，就直接让系统去抛吧。
            file = new FileStream(filePath + "/LCLFunctionDelegate.cs", FileMode.Create);
            sw = new StreamWriter(file);
            sw.Write(outputString);
            sw.Flush();
            sw.Close();
            file.Close();
        }
    }

}