using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.CLR.Method;
namespace ILRuntime.Runtime.Enviorment
{
    
    //public unsafe class ILContext
    //{
    //    public AppDomain AppDomain { get; private set; }
    //    public StackObject* ESP { get; private set; }
    //    public IList<object> ManagedStack { get; private set; }
    //    public IMethod Method { get; private set; }
    //    public ILIntepreter Interpreter { get; private set; }

    //    internal ILContext(AppDomain domain,ILIntepreter intpreter, StackObject* esp, IList<object> mStack, IMethod method)
    //    {
    //        AppDomain = domain;
    //        ESP = esp;
    //        ManagedStack = mStack;
    //        Method = method;
    //        Interpreter = intpreter;
    //    }
    //}

    public unsafe struct ILContext
    {
        private AppDomain _AppDomain;
        public AppDomain AppDomain { get { return _AppDomain; } private set { _AppDomain = value; } }
        private StackObject* _ESP;
        public StackObject* ESP { get { return _ESP; } private set { _ESP = value; } }
        private List<object> _ManagedStack;
        public List<object> ManagedStack { get { return _ManagedStack; } private set { _ManagedStack = value; } }
        private IMethod _Method;
        public IMethod Method { get { return _Method; } private set { _Method = value; } }
        private ILIntepreter _Interpreter;
        public ILIntepreter Interpreter { get { return _Interpreter; } private set { _Interpreter = value; } }

        internal ILContext(AppDomain domain, ILIntepreter intpreter, StackObject* esp, List<object> mStack, IMethod method)
        {
            _AppDomain = domain;
            _ESP = esp;
            _ManagedStack = mStack;
            _Method = method;
            _Interpreter = intpreter;
        }
    }
}
