//using System;
//using System.Collections;
//using System.Collections.Generic;
//using ILRuntime.CLR.Method;
//using ILRuntime.Runtime.Enviorment;
//using ILRuntime.Runtime.Intepreter;

//public class ISerializePacketAdapter : CrossBindingAdaptor
//{
//    public override Type BaseCLRType
//    {
//        get
//        {
//            return typeof(ISerializePacket);//这是你想继承的那个类
//        }
//    }

//    public override Type AdaptorType
//    {
//        get
//        {
//            return typeof(Adaptor);//这是实际的适配器类
//        }
//    }

//    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
//    {
//        return new Adaptor(appdomain, instance);//创建一个新的实例
//    }

//    //实际的适配器类需要继承你想继承的那个类，并且实现CrossBindingAdaptorType接口
//    class Adaptor : ISerializePacket, CrossBindingAdaptorType
//    {
//        ILTypeInstance instance;
//        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
//        //缓存这个数组来避免调用时的GC Alloc
//        object[] param1 = new object[1];

//        public Adaptor()
//        {

//        }
//        public Adaptor(ushort command):base(command)
//        {

//        }
//        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
//        {
//            this.appdomain = appdomain;
//            this.instance = instance;
//        }

//        public ILTypeInstance ILInstance { get { return instance; } }

//        //你需要重写所有你希望在热更脚本里面重写的方法，并且将控制权转到脚本里去
//        bool m_bSerializeGot = false;
//        IMethod m_Serialize = null;
//        public override void Serialize(WfPacket w)
//        {
//            if (!m_bSerializeGot)
//            {
//                m_Serialize = instance.Type.GetMethod("Serialize", 1);
//                m_bSerializeGot = true;
//            }
//            if (m_Serialize != null)
//            {
//                appdomain.Invoke(m_Serialize, instance, w);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
//            }
//        }

//        bool m_bDeSerializeGot = false;
//        IMethod m_DeSerialize = null;
//        public override void DeSerialize(WfPacket r)
//        {
//            if (!m_bDeSerializeGot)
//            {
//                m_DeSerialize = instance.Type.GetMethod("DeSerialize", 1);
//                m_bDeSerializeGot = true;
//            }
//            if (m_DeSerialize != null)
//            {
//                appdomain.Invoke(m_DeSerialize, instance, r);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
//            }
//        }
//    }
//}