using ILRuntime.Runtime.Enviorment;
using UnityEngine;

public class Lesson13 : MonoBehaviour
{
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            appDomain.Invoke("HotFix_Project.SelfLearn.ILRuntimeMain","Start",null,null);
        });
    }

    public static int TestFun(int i,int j)
    {
        return i + j;
    }
}
