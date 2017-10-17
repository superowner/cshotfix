using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace TestCreateDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllpath = "C:\\CSHotFix\\trunk\\Sample\\InjectGen\\bin\\Debug\\InjectGen.dll";
            var reader_parameter = new ReaderParameters();
            reader_parameter.ReadSymbols = true;
            var assembly_definition = AssemblyDefinition.ReadAssembly(dllpath, reader_parameter);
            //先清理所有的类型，确保每次都是全新注入
            var objType = assembly_definition.MainModule.ImportReference(typeof(MulticastDelegate));

            string delegate_name = "void_delegate";
            TypeDefinition td = new TypeDefinition("HotFix.HotFixDelegate", delegate_name, Mono.Cecil.TypeAttributes.Public, objType);
            assembly_definition.MainModule.Types.Add(td);

            var writerParameters = new WriterParameters { WriteSymbols = true };
            assembly_definition.Write(dllpath, writerParameters);
            if (assembly_definition.MainModule.SymbolReader != null)
            {
                assembly_definition.MainModule.SymbolReader.Dispose();
            }

        }
    }
}
