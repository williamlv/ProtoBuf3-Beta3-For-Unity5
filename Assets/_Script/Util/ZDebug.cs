using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;


public class ZDebug
{
    public static bool BreakOnAsserts = false;

    #region Assert
    [Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        if (condition)
        {
            if (BreakOnAsserts) UnityEngine.Debug.Break();
            throw new Exception();
        }
    }
    [Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string msg)
    {
        if (condition)
        {
            if (BreakOnAsserts) UnityEngine.Debug.Break();
            throw new Exception(msg);
        }
    }
    #endregion

    #region Console Logger

    public enum OutputType
    {
        Console,
        File,
        Both,
    }
    public static OutputType DisplayType = OutputType.Both;

    public enum ConsoleLogMethod
    {
        Silent, // no log calls recognized
        Selected, // recognize log calls made from types registered with LogThis(type)
        Verbose, // all log calls recognized
    }

    /// <summary>
    /// Public setter for setting logging verbosity
    /// </summary>
    public static ConsoleLogMethod DisplayMethod = ConsoleLogMethod.Verbose;

    private static readonly List<Type> SelectedTypes = new List<Type>();

    /// <summary>
    /// add a type into the selected logging types:
    /// CDebug.SelectType(typeof(ClassName));
    /// or
    /// CDebug.SelectType<ClassName>();
    /// </summary>
    /// <param name="type">log from this type</param>
    public static void SelectType(Type type)
    {
        if (!SelectedTypes.Contains(type))
        {
            SelectedTypes.Add(type);
        }
    }
    public static void SelectType<T>()
    {
        SelectType(typeof(T));
    }
    static List<string> mLines = new List<string>();
    static List<string> mWriteTxt = new List<string>();
    private static string outpath;
    public static void Init(bool breakOnAsset = false, ConsoleLogMethod displayMethod = ConsoleLogMethod.Verbose, OutputType displayType = OutputType.Both)
    {
        BreakOnAsserts = breakOnAsset;
        DisplayMethod = displayMethod;
        DisplayType = displayType;
        //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
        outpath = Application.persistentDataPath + "/outLog.txt";
        //每次启动客户端删除之前保存的Log
        if (System.IO.File.Exists(outpath))
        {
            File.Delete(outpath);
        }
        //在这里做一个Log的监听
        Application.logMessageReceived += HandleLog;
        //一个输出
        UnityEngine.Debug.Log(outpath);
        App.Instance().onUpdate += Update;
        App.Instance().onGUI += OnGUI;
    }
    #region UnityEngine.Debug.Log Wrappers
    /// <summary>
    /// 会输出到屏幕和文件的错误信息,程序会中断
    /// </summary>
    /// <param name="str">输出内容</param>
    public static void LogError(string str)
    {
        UnityEngine.Debug.LogError(str);
    }

    public static void LogError(string str, UnityEngine.Object obj)
    {
        UnityEngine.Debug.LogError(str, obj);
    }
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }

    public static void LogException(Exception exception, UnityEngine.Object obj)
    {
        UnityEngine.Debug.LogError(exception, obj);
    }
    public static void LogWarning(string msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }

    public static void LogWarning(string msg, UnityEngine.Object obj)
    {
        UnityEngine.Debug.LogWarning(msg, obj);
    }


    /// <summary>
    /// GameStart call ZDebug.Init();
    /// Default is DEBUG type, all log output on console and file
    /// if RELEASE Please Set follow type, then only selecttype will output.
    /// ZDebug.DisplayType = ZDebug.OutputType.File;
    /// ZDebug.DisplayMethod = ZDebug.ConsoleLogMethod.Selected;
    /// </summary>
    /// <param name="str"></param>
    public static void Log(string str)
    {
        if (CanLog())
        {
            UnityEngine.Debug.Log(str);
        }
    }
    public static void Log(string str, UnityEngine.Object obj)
    {
        if (CanLog())
        {
            UnityEngine.Debug.Log(str, obj);
        }
    }
    #endregion

    private static bool CanLog()
    {
        switch (DisplayMethod)
        {
            case ConsoleLogMethod.Selected:
                {
                    // call into the second level of StackFrame
                    // This -> CDebug -> Caller
                    var frame = new StackFrame(2);
                    var method = frame.GetMethod();
                    var type = method.DeclaringType.DeclaringType;
                    return SelectedTypes.Contains(type);
                }
            case ConsoleLogMethod.Verbose:
                {
                    return true;
                }
            case ConsoleLogMethod.Silent:
                {
                    return false;
                }
        }

        return false;
    }
    #endregion


    static void HandleLog(string logString, string stackTrace, LogType type)
    {
        mWriteTxt.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" + logString);
        if (type == LogType.Error || type == LogType.Exception)
        {
            GUILogErrorOutput(logString);
            GUILogErrorOutput(stackTrace);
        }
    }

    //这里我把错误的信息保存起来，用来输出在手机屏幕上
    static void GUILogErrorOutput(params object[] objs)
    {
        string text = "";
        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }
        if (Application.isPlaying)
        {
            if (mLines.Count > 20)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);

        }
    }

    static void Update()
    {
        //因为写入文件的操作必须在主线程中完成，所以在Update中哦给你写入文件。
        if (mWriteTxt.Count > 0 && (DisplayType == OutputType.File || DisplayType == OutputType.Both))
        {
            string[] temp = mWriteTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteTxt.Remove(t);
            }
        }
    }

    static void OnGUI()
    {
        if (DisplayType == OutputType.File)
            return;
        GUI.color = Color.red;
        for (int i = 0, imax = mLines.Count; i < imax; ++i)
        {
            GUILayout.Label(mLines[i]);
        }
    }
}