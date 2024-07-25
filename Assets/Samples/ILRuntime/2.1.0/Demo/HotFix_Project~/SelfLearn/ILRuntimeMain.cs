using UnityEngine;

namespace HotFix_Project.SelfLearn
{
    public delegate void MyILRuntimeDel1();
    public delegate int MyILRuntimeDel2(int i,int j);
    internal class ILRuntimeMain
    {
        public static void Start()
        {
            #region Lesson9
            //GameObject obj = new GameObject("ILRuntime Creates.");
            //obj.transform.position = new Vector3(10,10,10);
            //Debug.Log(obj.transform.position);
            //obj.AddComponent<Rigidbody>().mass = 9999;
            #endregion
            #region Lesson10
            ////1.ILRuntime中声明委托成员，关联ILRuntime中的函数
            //MyUnityDel1 fun1 = Fun1;
            //fun1.Invoke();
            //MyUnityDel2 fun2 = Fun2;
            //int resurlt = fun2.Invoke(5,6);
            //Debug.Log(resurlt);

            ////2.2.Unity中声明委托成员，关联ILRuntime中的函数
            //Lesson10 lesson10 = Camera.main.GetComponent<Lesson10>();
            ////自定义委托（需要委托转换器）
            //lesson10.fun1 = Fun1;
            //lesson10.fun1.Invoke();
            //lesson10.fun2 = Fun2;
            //resurlt = lesson10.fun2.Invoke(7,7);
            //Debug.Log(resurlt);
            ////系统委托（不需要注册委托转换器）
            //lesson10.funAction = Fun1;
            //lesson10.funAction.Invoke();
            //lesson10.funFunc = Fun2;
            //resurlt = lesson10.funFunc.Invoke(8,8);
            //Debug.Log(resurlt);

            ////3.ILRuntime中声明委托成员，关联ILRuntime中的函数（不要关联Unity中的，因为热更新发生在Unity项目完整结束后，此时Unity项目已经是写好的了，要关联Unity，那就又要去Unity那里去关联，不符合实际）
            //MyILRuntimeDel1 funIL1 = Fun1;
            //MyILRuntimeDel2 funIL2 = Fun2;
            //funIL1.Invoke();
            //resurlt = funIL2.Invoke(9,9);
            //Debug.Log(resurlt);

            #endregion
            #region Lesson11
            //TestLesson11 testLesson11 = new TestLesson11();
            //testLesson11.TestFun("哈哈哈");
            //testLesson11.TestAbstract(99);
            //testLesson11.valuePublic = 100;
            #endregion
            #region Lesson12
            //TestLesson12 testLesson12 = new TestLesson12();
            //testLesson12.Speak();
            #endregion
            #region Lesson13
            System.DateTime currentTime = System.DateTime.Now;
            for(int i = 0;i < 100000;i++)
            {
                Lesson13.TestFun(i,i);
            }
            Debug.Log("花费的时间： " + (System.DateTime.Now - currentTime).TotalMilliseconds + "ms");
            #endregion
        }
        public static void Fun1()
        {
            Debug.Log("IL_Fun1");
        }
        public static int Fun2(int a,int b)
        {
            Debug.Log("IL_Fun2");
            return a + b;
        }
    }
}
