using System.Collections;

public class HotFixDelegate
{
    public delegate void CtrDelegate(object param0);
    public delegate int DelegateImp0(object param0,string param1, int param2, out float param3);
}
public class HotFixFunction
{
    public static HotFixDelegate.CtrDelegate __hotfix__GameMono_GameLoop_ctr0;
    public static HotFixDelegate.DelegateImp0 __hotfix__GameMono__GameLoop__HelloWorld0;
}