using ILRuntime.Runtime.Enviorment;
using UnityEngine;

public class Lesson11 : MonoBehaviour
{
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            appDomain.Invoke("HotFix_Project.SelfLearn.ILRuntimeMain","Start",null,null);
        });
    }
}
