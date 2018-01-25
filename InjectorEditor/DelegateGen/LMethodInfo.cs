using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum RefOutArrayEnum
{
    None,
    Ref,
    Out,
    Array
}
public class ParamData
{
    public RefOutArrayEnum m_RefOut = RefOutArrayEnum.None;
    public string m_ParamString;
}
public class LMethodInfo
{
    public string m_ReturnString;
    public List<ParamData> m_Params = new List<ParamData>();
}

