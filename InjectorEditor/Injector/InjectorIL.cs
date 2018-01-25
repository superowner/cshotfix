using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class InjectorIL
{
    private List<TypeDefinition> m_DelegateFunctions = null;
    public void InjectAssembly(string dllPath)
    {
        var readerParameters = new ReaderParameters { ReadSymbols = true };
        AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(dllPath, readerParameters);

        foreach (var module in assembly.Modules)
        {
            List<TypeDefinition> types = module.Types.ToList();
            TypeDefinition FunctionDelegate = types.Find((td) => { return td.FullName.Contains("FunctionDelegate"); });
            if(FunctionDelegate != null)
            {
                m_DelegateFunctions = FunctionDelegate.NestedTypes.ToList();
                if (m_DelegateFunctions!= null && m_DelegateFunctions.Count > 0)
                {
                    break;
                }
            }
        }


        foreach (var module in assembly.Modules)
        {
            foreach (var typ in module.Types.Where(InjectorConfig.Filter))
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



 //   IL_0000: nop
 //   IL_0001: ldarg.0
//    IL_0002: ldfld class FunctionDelegate/method_delegate1 Demo.GameMain::TestFunc_Delegate

 //   IL_0007: ldnull
 //   IL_0008: cgt.un
 //   IL_000a: stloc.0

 //   IL_000b: ldloc.0

 //   IL_000c: brfalse.s IL_001f


 //   IL_000e: nop
 //   IL_000f: ldarg.0

 //   IL_0010: ldfld class FunctionDelegate/method_delegate1 Demo.GameMain::TestFunc_Delegate

 //   IL_0015: ldarg.1

 //   IL_0016: ldarg.2

 //   IL_0017: callvirt instance float32 FunctionDelegate/method_delegate1::Invoke(int32, int64)

 //   IL_001c: stloc.1
	//IL_001d: br.s IL_0027


 //   IL_001f: ldc.r4 -1
	//IL_0024: stloc.1
	//IL_0025: br.s IL_0027


 //   IL_0027: ldloc.1
	//IL_0028: ret













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

            //一旦有ref out的话，下标就要从1开始
            bool hasOutRefArrayParameter = HasOutRefArrayParameter(method);
            for (int i = 0; i < method.Parameters.Count; i++)
            {
                //压入参数
                int paramIdx = hasOutRefArrayParameter ? (i + 1) : i;
                ilGenerator.InsertBefore(insertPoint, CreateLoadArg(ilGenerator, paramIdx));
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
    public  Instruction CreateLoadIntConst(ILProcessor ilGenerator, int c)
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
    public  Instruction CreateLoadArg(ILProcessor ilGenerator, int c)
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
        if(pd.IsOut)
        {
            return RefOutArrayEnum.Out;
        }
        else if(pd.ParameterType.IsByReference)
        {
            return RefOutArrayEnum.Ref;
        }
        else if(pd.ParameterType.IsArray)
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
        var t = m_DelegateFunctions.Find((df)=> 
        {
            MethodDefinition mdf = df.Methods.ToList().Find((md) => { return md.Name.Contains("Invoke"); });
            if (mdf.ReturnType.FullName != method.ReturnType.FullName)
            {
                return false;
            }

            var mdf_params = mdf.Parameters.ToList();
            var mdf_params2 = method.Parameters.ToList();
            if (mdf_params.Count != mdf_params2.Count)
            {
                return false;
            }
            for(int i=0;i<mdf_params.Count;++i)
            {
                if(GetParamTypeEnum(mdf_params[i])!= GetParamTypeEnum(mdf_params2[i]))
                {
                    return false;
                }
                if(mdf_params[i].ParameterType.FullName != mdf_params2[i].ParameterType.FullName)
                {
                    return false;
                }

            }
            return true;

        });
        return t;
    }


    public  string GenerateMethodName(MethodDefinition method)
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

