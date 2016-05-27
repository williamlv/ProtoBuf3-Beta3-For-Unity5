using UnityEngine;

/// <summary>
/// 需要使用Unity生命周期的单例模式
/// </summary>
namespace ZFramework
{
    public abstract class ZMonoSingleton<T> : MonoBehaviour where T : ZMonoSingleton<T>
    {
        protected static T instance = null;

        public static T Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (FindObjectsOfType<T>().Length > 1)
                {
                    ZDebug.Log("More than 1!");
                }
                return instance;
            }

            if (instance == null)
            {
                string instanceName = typeof(T).Name;
                ZDebug.Log("Instance Name: " + instanceName);
                GameObject instanceGO = GameObject.Find(instanceName);

                if (instanceGO == null)
                    instanceGO = new GameObject(instanceName);
                instance = instanceGO.AddComponent<T>();
                DontDestroyOnLoad(instanceGO);  // 不会被释放
                ZDebug.Log("Add New Singleton " + instance.name + " in Game!");
            }
            else
            {
                ZDebug.Log("Already exist: " + instance.name);
            }

            return instance;
        }


        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }

}