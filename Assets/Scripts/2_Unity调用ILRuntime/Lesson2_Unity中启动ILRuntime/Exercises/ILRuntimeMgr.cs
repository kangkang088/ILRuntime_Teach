using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Mono.Cecil.Pdb;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntimeNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ILRuntimeMgr : MonoBehaviour
{
    private static ILRuntimeMgr instance;

    public ILRuntime.Runtime.Enviorment.AppDomain appDomain;
    private MemoryStream dllStream;
    private MemoryStream pdbStream;
    private bool isStart = false;
    public static ILRuntimeMgr Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("ILRuntimeMgr");
                instance = obj.AddComponent<ILRuntimeMgr>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    /// <summary>
    /// 开启ILRuntime
    /// </summary>
    public void StartILRuntime(UnityAction callback = null)
    {
        if(!isStart)
        {
            appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            StartCoroutine(LoadUpdateInfo(callback));
            isStart = true;
        }
    }

    IEnumerator LoadUpdateInfo(UnityAction callback)
    {
#if UNITY_ANDROID
        UnityWebRequest reqDLL = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix_Project.dll");
#else
        UnityWebRequest reqDLL = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/HotFix_Project.dll");
#endif
        yield return reqDLL.SendWebRequest();

        if(reqDLL.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("DLL文件加载失败：" + reqDLL.responseCode + reqDLL.result);
        }
        byte[] dll = reqDLL.downloadHandler.data;
        reqDLL.Dispose();

#if UNITY_ANDROID
        UnityWebRequest reqPDB = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
        UnityWebRequest reqPDB = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/HotFix_Project.pdb");
#endif
        yield return reqPDB.SendWebRequest();

        if(reqPDB.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("PDB文件加载失败：" + reqPDB.responseCode + reqPDB.result);
        }
        byte[] pdb = reqPDB.downloadHandler.data;
        reqPDB.Dispose();

        //3.以流的形式获取到数据传递给AppDomain对象
        dllStream = new MemoryStream(dll);
        pdbStream = new MemoryStream(pdb);
        appDomain.LoadAssembly(dllStream,pdbStream,new PdbReaderProvider());

        IniteRuntime();

        ILRuntimeLoadOverDoSomthing();

        //ILRuntime启动完毕后想要执行的逻辑
        callback?.Invoke();
    }

    /// <summary>
    /// 初始化ILRuntime相关内容
    /// </summary>
    unsafe private void IniteRuntime()
    {
        //4.初始化ILRuntime相关信息（目前只需要告诉ILRuntime，Unity主线程的线程ID，主要是为了能够在Unity的Profiler剖析器窗口分析问题）
        appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;

        //其他初始化
        //委托转换器注册(无参无返回值类型委托不需要注册委托)
        appDomain.DelegateManager.RegisterDelegateConvertor<MyUnityDel1>((act) =>
        {
            return new MyUnityDel1(() =>
            {
                ((Action)act)();
            });
        });

        //委托注册
        appDomain.DelegateManager.RegisterFunctionDelegate<System.Int32,System.Int32,System.Int32>();
        //委托转换器注册
        appDomain.DelegateManager.RegisterDelegateConvertor<MyUnityDel2>((act) =>
        {
            return new MyUnityDel2((i,j) =>
            {
                return ((Func<System.Int32,System.Int32,System.Int32>)act)(i,j);
            });
        });

        //注册跨域继承适配器
        appDomain.RegisterCrossBindingAdaptor(new Lesson11_TestAdapter());
        appDomain.RegisterCrossBindingAdaptor(new Lesson12_InterfaceAdapter());
        appDomain.RegisterCrossBindingAdaptor(new Lesson12_BaseClassAdapter());

        //CLR重定向内容，要在CLR绑定之前 
        Type debugType = typeof(Debug);
        MethodInfo methodInfo = debugType.GetMethod("Log",new Type[] { typeof(object) });
        //进行CLR重定向
        appDomain.RegisterCLRMethodRedirection(methodInfo,MyLog);

        //注册CLR绑定相关的信息
        ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);
    }

    unsafe StackObject* MyLog(ILIntepreter __intp,StackObject* __esp,List<object> __mStack,CLRMethod __method,bool isNewObj)
    {
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        StackObject* __ret = ILIntepreter.Minus(__esp,1);

        ptr_of_this_method = ILIntepreter.Minus(__esp,1);
        System.Object @message = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method,__domain,__mStack),(ILRuntime.CLR.Utils.Extensions.TypeFlags)0);
        __intp.Free(ptr_of_this_method);

        //获取对应的行号等相关信息
        var stackTrace = __domain.DebugService.GetStackTrace(__intp);

        UnityEngine.Debug.Log(@message + "\n" + stackTrace);

        return __ret;
    }

    /// <summary>
    /// 热更新逻辑
    /// </summary>
    private void ILRuntimeLoadOverDoSomthing()
    {
        //5.执行热更新代码中的逻辑
        print("ILRuntime Initialized Successfully!");
    }

    /// <summary>
    /// 停止ILRuntime
    /// </summary>
    public void StopILRuntime()
    {
        if(dllStream != null)
            dllStream.Dispose();
        if(pdbStream != null)
            pdbStream.Dispose();
        dllStream = null;
        pdbStream = null;
        appDomain = null;
        isStart = false;
    }
}
