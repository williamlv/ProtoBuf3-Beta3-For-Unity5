using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

/// <summary>
/// 1.泛型
/// 2.反射
/// 3.抽象类
/// 4.命名空间
/// </summary>
namespace ZFramework
{
    public abstract class ZSingleton<T> where T : ZSingleton<T>
    {
        protected static T instance = null;

        protected ZSingleton()
        {
        }

        public static T Instance()
        {
            if (instance == null)
            {
                // 先获取所有非public的构造方法
                ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                // 从ctors中获取无参的构造方法
                ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                if (ctor == null)
                    throw new Exception("Non-public ctor() not found!");
                // 调用构造方法
                instance = ctor.Invoke(null) as T;
            }

            return instance;
        }
    }
}

////////测试用例///////////////
///using QFramework;  
// 1.需要继承ZSingleton。
// 2.需要实现非public的构造方法。
//public class XXXManager : ZSingleton<XXXManager>
//{
//    private XXXManager()
//    {
//        // to do ...
//    }
//}


//public static void main(string[] args)
//{
//    XXXManager.Instance().xxxyyyzzz();
//}
//////////////////////////////////////////////////////////////////////////