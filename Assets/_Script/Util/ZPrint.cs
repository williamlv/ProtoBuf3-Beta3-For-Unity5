using UnityEngine;
using System.Collections;
/// <summary>
/// 输出控制
/// </summary>
public class ZPrint
{

    /// <summary>
    /// 警告
    /// </summary>
    public static void Warning(object message)
    {
        if (APP_CONFIG.DEBUG)
        {
            Debug.LogWarning("@@@@Custom@@@@@ " + message);
        }
    }
    /// <summary>
    /// 报错
    /// </summary>
    public static void Error(object message)
    {
        if (APP_CONFIG.DEBUG)
        {
            Debug.LogError("@@@@Framework@@@@ " + message);
        }
    }

    /// <summary>
    /// 日志
    /// </summary>
    public static void Log(object message)
    {
        if (APP_CONFIG.DEBUG)
        {
            Debug.Log("@@@@Framework@@@@ " + message);
        }
    }
}