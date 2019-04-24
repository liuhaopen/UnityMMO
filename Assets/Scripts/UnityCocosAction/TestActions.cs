using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//随便创建一个cube然后挂上本脚本，运行就可以了。
public class TestActions : MonoBehaviour
{
    Cocos.ActionRunner runner;
    Dictionary<string, System.Action> func;
    void Start()
    {
        runner = gameObject.AddComponent<Cocos.ActionRunner>();

        func = new Dictionary<string, System.Action>();
        func.Add("MoveBy", TestMoveBy);
        func.Add("MoveTo", TestMoveTo);
        func.Add("Sequence", TestSequence);
        func.Add("Spawn", TestSpawn);
        func.Add("FadeIn", TestFadeIn);
        func.Add("FadeOut", TestFadeOut);
        func.Add("FadeTo", TestFadeTo);
        func.Add("Repeat", TestRepeat);
        func.Add("RepeatForever", TestRepeatForever);
    }

    void TestMoveBy()
    {
        var moveAction = Cocos.MoveBy.CreateLocal(1, new Vector3(50,30,40));
        runner.PlayAction(moveAction);
    }

    void TestMoveTo()
    {
        var moveAction = Cocos.MoveTo.CreateLocal(1, new Vector3(150,30,40));
        runner.PlayAction(moveAction);
    }

    //测试几个action连续播放
    void TestSequence()
    {
        var moveAction = Cocos.MoveBy.CreateLocal(1, new Vector3(50,30,40));
        var action = Cocos.Sequence.Create(moveAction, moveAction.Reverse() as Cocos.FiniteTimeAction);
        runner.PlayAction(action);
    }

    //测试两个action同时播放
    void TestSpawn()
    {
        var moveAction = Cocos.MoveBy.CreateLocal(1, new Vector3(10, -50, -30));
        var fadeOutAction = Cocos.FadeOut.Create(0.8f);
        var action = Cocos.Spawn.Create(moveAction, fadeOutAction);
        runner.PlayAction(action);
    }

    void TestFadeIn()
    {
        var action = Cocos.FadeIn.Create(1);
        runner.PlayAction(action);
    }

    void TestFadeOut()
    {
        var action = Cocos.FadeOut.Create(1);
        runner.PlayAction(action);
    }

    void TestFadeTo()
    {
        var action = Cocos.FadeTo.Create(1, 255/2);
        runner.PlayAction(action);
    }

    void TestRepeat()
    {
        var moveAction = Cocos.MoveBy.CreateLocal(1, new Vector3(50,30,40));
        var sequenceAction = Cocos.Sequence.Create(moveAction, moveAction.Reverse() as Cocos.FiniteTimeAction);
        var repeatAction = Cocos.Repeat.Create(sequenceAction, 2);
        runner.PlayAction(repeatAction);
    }

    void TestRepeatForever()
    {
        var moveAction = Cocos.MoveBy.CreateLocal(1, new Vector3(50,30,40));
        var sequenceAction = Cocos.Sequence.Create(moveAction, moveAction.Reverse() as Cocos.FiniteTimeAction);
        var repeatAction = Cocos.RepeatForever.Create(sequenceAction);
        runner.PlayAction(repeatAction);
    }

    float BtnWidth = 80;
    float BtnHeight = 40;
    Vector2 scrollPos = Vector2.zero;

    private void OnGUI() 
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(100));
        GUILayout.BeginVertical();
        foreach (var item in func)
        {
            bool isClick = GUILayout.Button(item.Key, GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight));
            if (isClick)
                item.Value();
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
