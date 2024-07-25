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

            //1.获取方法信息
            IMethod getStr = type.GetMethod("get_Str",0);
            IMethod setStr = type.GetMethod("set_Str",1);
            //2.调用方法
            //方式一(不建议)
            string str = appDomain.Invoke(getStr,obj) as string;
            print(str);
            appDomain.Invoke(setStr,obj,"666");
            str = appDomain.Invoke(getStr,obj) as string;
            print(str);
            //方式二(建议)
            using(var setMethod = appDomain.BeginInvoke(setStr))
            {
                setMethod.PushObject(obj);
                string param = "789";
                setMethod.PushValueType<string>(ref param);
                setMethod.Invoke();
            }
            using(var getMethod = appDomain.BeginInvoke(getStr))
            {
                //传入执行该方法的对象
                getMethod.PushObject(obj);
                //执行方法
                getMethod.Invoke();
                //获取返回值
                str = getMethod.ReadValueType<string>();
                print(str);
            }
        });
    }
}