using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif

namespace ILRuntimeNamespace
{   
    public class Lesson11_TestAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::Lesson11_Test);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : global::Lesson11_Test, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<System.Int32> mget_ValuePro_0 = new CrossBindingFunctionInfo<System.Int32>("get_ValuePro");
            CrossBindingMethodInfo<System.Int32> mset_ValuePro_1 = new CrossBindingMethodInfo<System.Int32>("set_ValuePro");
            CrossBindingMethodInfo<System.String> mTestFun_2 = new CrossBindingMethodInfo<System.String>("TestFun");
            CrossBindingMethodInfo<System.Int32> mTestAbstract_3 = new CrossBindingMethodInfo<System.Int32>("TestAbstract");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void TestFun(System.String str)
            {
                if (mTestFun_2.CheckShouldInvokeBase(this.instance))
                    base.TestFun(str);
                else
                    mTestFun_2.Invoke(this.instance, str);
            }

            public override void TestAbstract(System.Int32 i)
            {
                mTestAbstract_3.Invoke(this.instance, i);
            }

            public override System.Int32 ValuePro
            {
            get
            {
                if (mget_ValuePro_0.CheckShouldInvokeBase(this.instance))
                    return base.ValuePro;
                else
                    return mget_ValuePro_0.Invoke(this.instance);

            }
            set
            {
                if (mset_ValuePro_1.CheckShouldInvokeBase(this.instance))
                    base.ValuePro = value;
                else
                    mset_ValuePro_1.Invoke(this.instance, value);

            }
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

