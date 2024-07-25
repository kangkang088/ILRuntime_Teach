using ILRuntime.Runtime.Enviorment;
using UnityEngine;

public class Lesson9 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;

            appDomain.Invoke("HotFix_Project.SelfLearn.ILRuntimeMain","Start",null,null);
        });
    }
}
