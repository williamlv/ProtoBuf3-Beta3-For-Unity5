using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 状态机实现
/// </summary>
public class FSM
{
    // 定义函数指针类型
    public delegate void FSMCallfunc();

    /// <summary>
    /// 状态类
    /// </summary>
    public class FSMState
    {
        public string name;

        public FSMState(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 存储事件对应的条转
        /// </summary>
        public Dictionary<string, FSMTranslation> TranslationDict = new Dictionary<string, FSMTranslation>();
    }

    /// <summary>
    /// 跳转类
    /// </summary>
    public class FSMTranslation
    {
        public string fromState;
        public string name;
        public string toState;
        public FSMCallfunc callfunc;    // 回调函数

        public FSMTranslation(string fromState, string name, string toState, FSMCallfunc callfunc)
        {
            this.fromState = fromState;
            this.toState = toState;
            this.name = name;
            this.callfunc = callfunc;
        }
    }

    // 当前状态
    private string mCurState;

    public string State
    {
        get
        {
            return mCurState;
        }
    }

    // 状态
    Dictionary<string, FSMState> StateDict = new Dictionary<string, FSMState>();

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="state">State.</param>
    public void AddState(string name)
    {
        StateDict[name] = new FSMState(name);
    }

    /// <summary>
    /// 添加条转
    /// </summary>
    /// <param name="translation">Translation.</param>
    public void AddTranslation(string fromState, string name, string toState, FSMCallfunc callfunc)
    {
        StateDict[fromState].TranslationDict[name] = new FSMTranslation(fromState, name, toState, callfunc);
    }

    /// <summary>
    /// 启动状态机
    /// </summary>
    /// <param name="state">State.</param>
    public void Start(string name)
    {
        mCurState = name;
    }


    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="name">Name.</param>
    public void HandleEvent(string name)
    {
        if (mCurState != null && StateDict[mCurState].TranslationDict.ContainsKey(name))
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            FSMTranslation tempTranslation = StateDict[mCurState].TranslationDict[name];
            tempTranslation.callfunc();
            mCurState = tempTranslation.toState;

            watch.Stop();
        }
    }

    public void Clear()
    {
        StateDict.Clear();
    }
}



/////////////////////测试代码
//        Idle,               闲置
//        Run,                跑
//        Jump,               一段跳
//        DoubleJump,         二段跳
//        Die,                挂彩

// 创建状态
/*
FSM.FSMState idleState = new FSM.FSMState("idle");
FSM.FSMState runState = new FSM.FSMState("run");
FSM.FSMState jumpState = new FSM.FSMState("jump");
FSM.FSMState doubleJumpState = new FSM.FSMState("double_jump");
FSM.FSMState dieState = new FSM.FSMState("die");
// 创建跳转
FSM.FSMTranslation touchTranslation1 = new FSM.FSMTranslation(runState, "touch_down", jumpState, Jump);
FSM.FSMTranslation touchTranslation2 = new FSM.FSMTranslation(jumpState, "touch_down", doubleJumpState, DoubleJump);

FSM.FSMTranslation landTranslation1 = new FSM.FSMTranslation(jumpState, "land", runState, Run);
FSM.FSMTranslation landTranslation2 = new FSM.FSMTranslation(doubleJumpState, "land", runState, Run);

// 添加状态
PlayerModel.Instance ().fsm.AddState (idleState);
PlayerModel.Instance ().fsm.AddState (runState);
PlayerModel.Instance ().fsm.AddState (jumpState);
PlayerModel.Instance ().fsm.AddState (doubleJumpState);
PlayerModel.Instance ().fsm.AddState (dieState);

// 添加跳转
PlayerModel.Instance ().fsm.AddTranslation (touchTranslation1);
PlayerModel.Instance ().fsm.AddTranslation (touchTranslation2);
PlayerModel.Instance ().fsm.AddTranslation (landTranslation1);
PlayerModel.Instance ().fsm.AddTranslation (landTranslation2);

PlayerModel.Instance ().fsm.Start (runState);

    */
