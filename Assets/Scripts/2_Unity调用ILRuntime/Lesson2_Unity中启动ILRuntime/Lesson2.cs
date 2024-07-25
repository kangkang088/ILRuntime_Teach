using System.Collections;
using System.IO;
using System.Threading;
using ILRuntime.Mono.Cecil.Pdb;
using ILRuntime.Runtime.Enviorment;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson2 : MonoBehaviour
{
    private AppDomain appDomain;
    private MemoryStream dllStream;
    private MemoryStream pdbStream;
    // Start is called before the first frame update
    void Start()
    {
        //1.声明并初始化类对象
        appDomain = new AppDomain();
        //2.加载本地或远端下载的DLL文件和PDB文件
        StartCoroutine(LoadUpdateInfo());
    }

    /// <summary>
    /// 异步加载热更相关的DLL文件和PDB文件
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadUpdateInfo()
    {
#if UNITY_ANDROID
        UnityWebRequest reqDLL = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix_Project.dll");
#else
        UnityWebRequest reqDLL = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/HotFix_Project.dll");
#endif
        yield return reqDLL.SendWebRequest();

        if (reqDLL.result != UnityWebRequest.Result.Success)
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

        if (reqPDB.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("PDB文件加载失败：" + reqPDB.responseCode + reqPDB.result);
        }
        byte[] pdb = reqPDB.downloadHandler.data;
        reqPDB.Dispose();

        //3.以流的形式获取到数据传递给AppDomain对象
        dllStream = new MemoryStream(dll);
        pdbStream = new MemoryStream(pdb);
        appDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());

        //4.初始化ILRuntime相关信息（目前只需要告诉ILRuntime，Unity主线程的线程ID，主要是为了能够在Unity的Profiler剖析器窗口分析问题）
        appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;

        //5.执行热更新代码中的逻辑
        print("ILRuntime Initialized Successfully!");
    }

    void OnDestroy()
    {
        if (dllStream != null)
            dllStream.Dispose();
        if (pdbStream != null)
            pdbStream.Dispose();
        dllStream = null;
        pdbStream = null;
        appDomain = null;
    }
}
