using UnityEngine;

namespace HotFix_Project.SelfLearn
{
    internal class TestLesson11 : Lesson11_Test
    {
        public override int ValuePro
        {
            get => valueProtected;
            set => valueProtected = value;
        }
        public override void TestFun(string str)
        {
            base.TestFun(str);
            Debug.Log("TestFun_IL: " + str);
        }
        public override void TestAbstract(int i)
        {
            Debug.Log("TestAbstract_IL: " + i);
        }

        public void Speak()
        {
            Debug.Log("Speak");
        }
    }
}
