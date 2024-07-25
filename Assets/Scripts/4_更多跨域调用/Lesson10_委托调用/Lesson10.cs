using System;
using UnityEngine;

public delegate void MyUnityDel1();
public delegate int MyUnityDel2(int i,int j);

public class Lesson10 : MonoBehaviour
{
    public MyUnityDel1 fun1;
    public MyUnityDel2 fun2;

    public Action funAction;
    public Func<int,int,int> funFunc;
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            ILRuntime.Runtime.Enviorment.AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;

            appDomain.Invoke("HotFix_Project.SelfLearn.ILRuntimeMain","Start",null,null);


        });
    }
}
