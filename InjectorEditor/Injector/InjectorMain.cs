using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class InjectorMain
{
    public void Run(string dllPath)
    {
        InjectorIL IL = new InjectorIL();
        IL.InjectAssembly(dllPath);
    }
}

