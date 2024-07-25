using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections.Generic;
using UnityEngine;

public class Lesson8 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            IType type = appDomain.LoadedTypes["HotFix_Project.SelfLearn.Lesson3_Test"];
            object obj = ((ILType)type).Instantiate();

            IMethod methodName = type.GetMethod("TestFun3",3);

            List<int> list = new() { 1,2,3,4 };
            using(var method = appDomain.BeginInvoke(methodName))
            {
                //ѹ��ref��ʼֵ
                method.PushObject(list);
                //ѹ��out��ʼֵ
                method.PushObject(null);

                //ѹ����ö���
                method.PushObject(obj);
                method.PushInteger(100);

                //ѹ��ref/out����
                method.PushReference(0);
                method.PushReference(1);

                method.Invoke();

                //��˳���ȡref/out��ֵ������ȡ��������ֵ
                List<int> refValue = method.ReadObject<List<int>>(0);
                float outValue = method.ReadFloat(1);
                float returnValue = method.ReadFloat();
                print(refValue.Count);
                print(outValue);
                print(returnValue);
            }
        });
    }
}
