using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using UnityEngine;

public class Lesson5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            //方式一
            appDomain.Invoke("HotFix_Project.SelfLearn.Lesson3_Test","TestStaticFun1",null,null);
            int value1 = (int)(appDomain.Invoke("HotFix_Project.SelfLearn.Lesson3_Test","TestStaticFun2",null,90));
            print(value1);
            //方式二
            IType type = appDomain.LoadedTypes["HotFix_Project.SelfLearn.Lesson3_Test"];
            IMethod method1 = type.GetMethod("TestStaticFun1",0);
            IMethod method2 = type.GetMethod("TestStaticFun2",1);
            appDomain.Invoke(method1,null,null);
            int value2 = (int)(appDomain.Invoke(method2,null,900));
            print(value2);
            //方式三(推荐方式)
            using(var method_1 = appDomain.BeginInvoke(method1))
            {
                method_1.Invoke();
            }
            using(var method_2 = appDomain.BeginInvoke(method2))
            {
                method_2.PushInteger(77);
                method_2.Invoke();
                value2 = method_2.ReadInteger();
                print(value2);
            }
        });
    }
}
