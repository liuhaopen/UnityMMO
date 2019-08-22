using UnityEngine;

namespace UnityMMO
{
public class FightFlyWord : MonoBehaviour {
    Cocos.ActionRunner runner;
    TMPro.TextMeshPro label;

    private void Awake() 
    {
        runner = gameObject.AddComponent<Cocos.ActionRunner>();
        label = gameObject.GetComponent<TMPro.TextMeshPro>();
    }

    private void Update() 
    {
        //始终面向摄像机
        transform.rotation = Camera.main.transform.rotation;
    }

    public void StartFly()
    {
        var randomX = Random.RandomRange(-1.0f, 1.0f);
        var randomY = Random.RandomRange(1.5f, 2.5f);
        var randomZ = Random.RandomRange(-1.0f, 1.0f);
        var randomMoveDuration = Random.RandomRange(0.5f, 1.0f);
        var moveAction = Cocos.MoveBy.CreateAbs(randomMoveDuration, new Vector3(randomX, randomY, randomZ));
        var fadeOutAction = Cocos.FadeOut.Create(0.7f, Cocos.ColorAttrCatcherTextMeshPro.Ins);
        var delayFadeoutAction = Cocos.Sequence.Create(Cocos.DelayTime.Create(0.5f), fadeOutAction);
        var spawnAction = Cocos.Spawn.Create(moveAction, delayFadeoutAction);
        var action = Cocos.Sequence.Create(spawnAction, Cocos.CallFunc.Create(()=>{
            // Object.Destroy(gameObject, 0.1f);
            gameObject.SetActive(false);
            Cocos.Helper.SetOpacity(transform, 1, Cocos.ColorAttrCatcherTextMeshPro.Ins);
            ResMgr.GetInstance().UnuseGameObject("FightFlyWord", gameObject);
        }));
        runner.PlayAction(action);
    }
    
    public void SetData(long num, long flag)
    {
        string showStr = "";
        string numStyle = "";
        if (flag == 0)
        {
            numStyle = "damage_mainrole_";
        }
        else if (flag == 1)
        {
            numStyle = "baoji_mainrole_";
            showStr = "<sprite name=\"baoji_mainrole\">";
        }
        else if (flag == 2)
        {
            numStyle = "baoji_other_";
        }
        string numStr = num.ToString();
        for (int i = 0; i < numStr.Length; i++)
        {
            showStr += "<sprite name=\""+numStyle+numStr[i]+"\">";
        }
        label.text = showStr;
    }

}

}