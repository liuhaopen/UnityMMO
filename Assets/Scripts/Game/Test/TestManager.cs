using SprotoType;
using UnityEngine;

namespace UnityMMO{
public class TestManager : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyUp(UnityEngine.KeyCode.U))
        {
            Debug.Log("key U up");
            // SceneMgr.Instance.LoadScene(2001);
        //     account_select_role_enter_game.request req = new account_select_role_enter_game.request();
        //     req.role_id = 123654;
        //     NetMsgDispatcher.GetInstance().SendMessage<Protocol.account_select_role_enter_game>(req, (_) =>
        //     {
        //         account_select_role_enter_game.response rsp = _ as account_select_role_enter_game.response;
        //         Debug.Log("rsp.result : "+rsp.result.ToString());
        //         if (rsp.result == 0)
        //         {
        //         }
        //     });
        }
    }
}
}