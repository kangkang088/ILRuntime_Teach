using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using UnityEngine;

public class Lesson6 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            IType type = appDomain.LoadedTypes["HotFix_Project.SelfLearn.Lesson3_Test"];
            object obj = ((ILType)type).Instantiate();

            //方式一
            appDomain.Invoke("HotFix_Project.SelfLearn.Lesson3_Test","TestFun1",obj,null);
            int value = (int)appDomain.Invoke("HotFix_Project.SelfLearn.Lesson3_Test","TestFun2",obj,99);
            print(value);
            //方式二
            IMethod method1 = type.GetMethod("TestFun1",0);
            IMethod method2 = type.GetMethod("TestFun2",1);
            appDomain.Invoke(method1,obj);
            value = (int)appDomain.Invoke(method2,obj,88);
            print(value);
            //方式三
            using(var method_1 = appDomain.BeginInvoke(method1))
            {
                method_1.PushObject(obj);
                method_1.Invoke();
            }
            using(var method_2 = appDomain.BeginInvoke(method2))
            {
                method_2.PushObject(obj);
                method_2.PushInteger(77);
                method_2.Invoke();
                value = method_2.ReadValueType<int>();
                print(value);
            }
        });
    }
}
