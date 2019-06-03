using UnityEngine;

public class ReloadLayoutOnExitGame : MonoBehaviour
{
#if UNITY_EDITOR
    private bool hadSaveOnRunTime = false;
    private bool isRunningGame = false;

    public bool SetHadSaveOnRunTime(bool value)
    {
        if (isRunningGame)
            hadSaveOnRunTime = value;

        return hadSaveOnRunTime;
    }

    private void Start()
    {
        hadSaveOnRunTime = false;
        isRunningGame = true;
        //Debug.Log("ReloadLayoutOnExitGame Start()");
    }
    
    //after exit game from unity editor, reload layouts which has been saved during the run
    private void OnApplicationQuit()
    {
        //Debug.Log("ReloadLayoutOnExitGame OnApplicationQuit()"+ hadSaveOnRunTime.ToString());
        if (hadSaveOnRunTime && U3DExtends.Configure.ReloadLayoutOnExitGame)
        {
            //因为本组件要正常地监听运行游戏和结束游戏事件,所以不能ExecuteInEditMode,而游戏运行结束最后一个事件是OnApplicationQuit,此事件后unity才会重置所有运行时的修改,所以我们需要延迟一段时间再重新加载界面,否则重新加载后又被重置了
            U3DExtends.UIEditorHelper.DelayReLoadLayout(gameObject, true);
            hadSaveOnRunTime = false;
        }
        isRunningGame = false;
    }
#endif
}