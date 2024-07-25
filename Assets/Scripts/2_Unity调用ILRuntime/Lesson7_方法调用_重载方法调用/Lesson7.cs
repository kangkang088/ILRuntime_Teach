using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections.Generic;
using UnityEngine;

public class Lesson7 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            IType type = appDomain.LoadedTypes["HotFix_Project.SelfLearn.Lesson3_Test"];
            object obj = ((ILType)type).Instantiate();

            //参数数量相同，类型不同时，无法区分
            appDomain.Invoke("HotFix_Project.SelfLearn.Lesson3_Test","TestFun1",obj);
            appDomain.Invoke("HotFix_Project.SelfLearn.Lesson3_Test","TestFun1",obj,1);
            //appDomain.Invoke("HotFix_Project.SelfLearn.Lesson3_Test","TestFun1",obj,1.1f);

            IMethod method1 = type.GetMethod("TestFun1",0);
            IMethod method2 = type.GetMethod("TestFun1",1);
            appDomain.Invoke(method1,obj);
            appDomain.Invoke(method2,obj,1);
            //appDomain.Invoke(method2,obj,1.1f);

            using(var method_1 = appDomain.BeginInvoke(method1))
            {
                method_1.PushObject(obj);
                method_1.Invoke();
            }
            using(var method_2 = appDomain.BeginInvoke(method2))
            {
                method_2.PushObject(obj);
                method_2.PushInteger(1);
                //method_2.PushFloat(1.1f);
                method_2.Invoke();
            }

            //参数数量相同，类型不同时，可以区分
            IType floatType = appDomain.GetType(typeof(float));
            List<IType> list = new()
            {
                floatType
            };
            method2 = type.GetMethod("TestFun1",list,null);
            using(var method_3 = appDomain.BeginInvoke(method2))
            {
                method_3.PushObject(obj);
                method_3.PushFloat(5.5f);
                method_3.Invoke();
            }
        });
    }
}
