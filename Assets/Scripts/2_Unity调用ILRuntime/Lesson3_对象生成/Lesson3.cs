using System;
using System.Reflection;
using ILRuntime.CLR.TypeSystem;
using UnityEngine;

public class Lesson3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            ILRuntime.Runtime.Enviorment.AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            //方式一：
            //无参构造
            object obj = appDomain.Instantiate("HotFix_Project.SelfLearn.Lesson3_Test");
            print(obj);
            //有参构造
            obj = appDomain.Instantiate("HotFix_Project.SelfLearn.Lesson3_Test", new object[] { "123" });
            print(obj);

            //方式二：
            //无参构造
            IType type = appDomain.LoadedTypes["HotFix_Project.SelfLearn.Lesson3_Test"];
            obj = ((ILType)type).Instantiate();
            print(obj);
            //有参构造
            obj = ((ILType)type).Instantiate(new object[] { "123" });
            print(obj);

            //方式三：
            //无参构造
            ConstructorInfo info = type.ReflectionType.GetConstructor(new Type[0]);
            obj = info.Invoke(null);
            print(obj);
            //有参构造
            info = type.ReflectionType.GetConstructor(new Type[] { typeof(string) });
            obj = info.Invoke(new object[] { "123-" });
            print(obj);
        });
    }
}
