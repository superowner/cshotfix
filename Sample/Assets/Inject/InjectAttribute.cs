using System;


namespace LCL
{
    public enum InjectFlag
    {
        NoInject = 0,
        Inject =1,
    }
    public class InjectAttribute : Attribute
    {
        InjectFlag flag;
        public InjectFlag Flag
        {
            get
            {
                return flag;
            }
        }

        public InjectAttribute(InjectFlag flag = InjectFlag.NoInject)
        {
            this.flag = flag;
        }
    }
}
