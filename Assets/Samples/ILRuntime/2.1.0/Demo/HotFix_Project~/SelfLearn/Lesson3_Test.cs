using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project.SelfLearn
{
    internal class Lesson3_Test
    {
        private string str;
        public Lesson3_Test()
        {
        }
        public Lesson3_Test(string str)
        {
            this.str = str;
        }

        #region Lesson4
        public string Str
        {
            get
            {
                return str;
            }
            set
            {
                str = value;
            }
        }
        #endregion
        #region Lesson5
        public static void TestStaticFun1()
        {
            Debug.Log("无参静态方法");
        }

        public static int TestStaticFun2(int i)
        {
            Debug.Log("有参静态方法 " + i);
            return i + 10;
        }
        #endregion
        #region Lesson6
        public void TestFun1()
        {
            Debug.Log("无参无返回值成员方法调用");
        }
        public int TestFun2(int i)
        {
            Debug.Log("有参有返回值成员方法调用");
            return i + 10;
        }
        #endregion
        #region Lesson7
        public void TestFun1(int i)
        {
            Debug.Log("重载函数1 " + i);
        }
        public void TestFun1(float i)
        {
            Debug.Log("重载函数2 " + i);
        }
        #endregion
        #region Lesson8
        public float TestFun3(int i,ref List<int> list,out float f)
        {
            f = 0.5f;
            list.Add(5);
            for(int j = 0;j < list.Count;j++)
            {
                Debug.Log(j);
            }
            return i + list.Count + f;
        }
        #endregion
    }
}
