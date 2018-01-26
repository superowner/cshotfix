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
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
namespace LCL
{
    public class InjectorIL
    {
        private List<TypeDefinition> m_DelegateFunctions = null;
        public void InjectAssembly(string dllPath)
        {
            var readerParameters = new ReaderParameters { ReadSymbols = false };
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(dllPath, readerParameters);

            foreach (var module in assembly.Modules)
            {
                List<TypeDefinition> types = module.Types.ToList();
                TypeDefinition FunctionDelegate = types.Find((td) => { return td.FullName.Contains("FunctionDelegate"); });
                if (FunctionDelegate != null)
                {
                    m_DelegateFunctions = FunctionDelegate.NestedTypes.ToList();
                    if (m_DelegateFunctions != null && m_DelegateFunctions.Count > 0)
                    {
                        break;
                    }
                }
            }


            foreach (var module in assembly.Modules)
            {
                foreach (var typ in module.Types)
                {
                    foreach (var method in typ.Methods)
                    {
                        InjectMethod(typ, method);
                    }
                }
            }

            var writerParameters = new WriterParameters { WriteSymbols = true };
            assembly.Write(dllPath, writerParameters);


            if (assembly.MainModule.SymbolReader != null)
            {
                assembly.MainModule.SymbolReader.Dispose();
            }
        }


        private void InjectMethod(TypeDefinition type, MethodDefinition method)
        {
            if (type.Name.Contains("<") || type.IsInterface || type.Methods.Count == 0) // skip anonymous type and interface
                return;
            if (method.Name == ".cctor")
                return;
            if (method.Name == ".ctor")
                return;

            //寻找一个用于注入的委托
            TypeDefinition delegateTypeRef = FindDelegateFunction(method);

            if (delegateTypeRef != null)
            {
                //在type里面定义一个字段，类型是我们刚刚找到的委托方法
                string delegateFieldName = GenerateMethodName(method);
                FieldDefinition item = new FieldDefinition(delegateFieldName, FieldAttributes.Static | FieldAttributes.Public, delegateTypeRef);
                type.Fields.Add(item);

                //找到委托的Invoke函数，并且导入到当前类
                var invokeDeclare = type.Module.ImportReference(delegateTypeRef.Methods.Single(x => x.Name == "Invoke"));

                if (!method.HasBody)
                    return;

                var insertPoint = method.Body.Instructions[0];
                var ilGenerator = method.Body.GetILProcessor();

                //压入delegate变量
                ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldsfld, item));
                //压入Null
                ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldnull));
                //压入比较符号
                ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Cgt_Un));

                //ilspy的误导，其实不用这么写
                //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Stloc_0));
                //ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldloc_0));

                //压入Ifelse语句
                ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Brfalse, insertPoint));


                //处理if大括号内部逻辑
                ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldsfld, item));


                if (method.IsStatic)
                {
                    //压入一个null
                    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ldnull));
                }
                else
                {
                    //压入this
                    ilGenerator.InsertBefore(insertPoint, CreateLoadArg(ilGenerator, 0));
                }

                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    //压入参数
                    ilGenerator.InsertBefore(insertPoint, CreateLoadArg(ilGenerator, method.IsStatic ? i : i + 1));
                }


                //调用委托
                ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Call, invokeDeclare));

                if (method.ReturnType.Name == "Void")
                {
                    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Nop));
                }
                else if (method.ReturnType.IsValueType)
                {
                    ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Unbox_Any, method.ReturnType));
                }
                ilGenerator.InsertBefore(insertPoint, ilGenerator.Create(OpCodes.Ret));
            }
        }
        private bool HasOutRefArrayParameter(MethodDefinition method)
        {
            return method.Parameters.ToList().Exists((pd) =>
            {
                return GetParamTypeEnum(pd) != RefOutArrayEnum.None;
            });
        }
        //常量入栈，常用于压入一个int常量等
        public Instruction CreateLoadIntConst(ILProcessor ilGenerator, int c)
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

        //参数列表使用的函数
        public Instruction CreateLoadArg(ILProcessor ilGenerator, int c)
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
        private RefOutArrayEnum GetParamTypeEnum(ParameterDefinition pd)
        {
            if (pd.IsOut)
            {
                return RefOutArrayEnum.Out;
            }
            else if (pd.ParameterType.IsByReference)
            {
                return RefOutArrayEnum.Ref;
            }
            else if (pd.ParameterType.IsArray)
            {
                return RefOutArrayEnum.Array;
            }
            else
            {
                return RefOutArrayEnum.None;
            }
        }
        private TypeDefinition FindDelegateFunction(MethodDefinition method)
        {
            var t = m_DelegateFunctions.Find((df) =>
            {
                MethodDefinition mdf = df.Methods.ToList().Find((md) => { return md.Name.Contains("Invoke"); });
                if (mdf.ReturnType.FullName != method.ReturnType.FullName)
                {
                    return false;
                }

                var mdf_params = mdf.Parameters.ToList();
                var mdf_params2 = method.Parameters.ToList();
                if (mdf_params.Count != mdf_params2.Count + 1)
                {
                    return false;
                }
                for (int i = 0; i < mdf_params2.Count; ++i)
                {
                    if (GetParamTypeEnum(mdf_params[i + 1]) != GetParamTypeEnum(mdf_params2[i]))
                    {
                        return false;
                    }
                    if (mdf_params[i + 1].ParameterType.FullName != mdf_params2[i].ParameterType.FullName)
                    {
                        return false;
                    }

                }
                return true;

            });
            return t;
        }


        public string GenerateMethodName(MethodDefinition method)
        {
            string delegateFieldName = "__" + method.Name;
            for (int i = 0; i < method.Parameters.Count; i++)
            {
                delegateFieldName += "_" + method.Parameters[i].ParameterType.Name;
            }
            delegateFieldName += "__Delegate";
            delegateFieldName = delegateFieldName.Replace(".", "_").
                Replace("`", "_").
                Replace("&", "_at_").
                Replace("[]", "_array_");

            return delegateFieldName;
        }
    }

}