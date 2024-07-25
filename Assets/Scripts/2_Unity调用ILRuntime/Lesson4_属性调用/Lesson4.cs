using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using UnityEngine;

public class Lesson4 : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            ILRuntime.Runtime.Enviorment.AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;

            IType type = appDomain.LoadedTypes["HotFix_Project.SelfLearn.Lesson3_Test"];
            object obj = ((ILType)type).Instantiate(new object[] { "234" });
            print(obj);

            //1.��ȡ������Ϣ
            IMethod getStr = type.GetMethod("get_Str",0);
            IMethod setStr = type.GetMethod("set_Str",1);
            //2.���÷���
            //��ʽһ(������)
            string str = appDomain.Invoke(getStr,obj) as string;
            print(str);
            appDomain.Invoke(setStr,obj,"666");
            str = appDomain.Invoke(getStr,obj) as string;
            print(str);
            //��ʽ��(����)
            using(var setMethod = appDomain.BeginInvoke(setStr))
            {
                setMethod.PushObject(obj);
                string param = "789";
                setMethod.PushValueType<string>(ref param);
                setMethod.Invoke();
            }
            using(var getMethod = appDomain.BeginInvoke(getStr))
            {
                //����ִ�и÷����Ķ���
                getMethod.PushObject(obj);
                //ִ�з���
                getMethod.Invoke();
                //��ȡ����ֵ
                str = getMethod.ReadValueType<string>();
                print(str);
            }
        });
    }
}