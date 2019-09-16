using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Profiling;
using UnityMMO;
using UnityMMO.Component;

namespace UnityMMO
{
    public class NameboardData : MonoBehaviour
    {
        public enum ResState 
        {
            WaitLoad,//等待判断是否离主角够近，够近才进入此状态等待加载prefab
            // Loading,//加载中
            Loaded,//已加载
            // Deleting,//远离主角，别加载了
            DontLoad,//不需要再加载了
        }
        public ResState UIResState;
        public Transform LooksNode;
        public string Name;
        public float Height;
        public NameboardData()
        {
            UIResState = ResState.WaitLoad;
            LooksNode = null;
            Name = "";
            Height = 1.6f;
        }

        public void SetName(string name)
        {
            // Debug.Log("nameboard name:"+name+" "+new System.Diagnostics.StackTrace().ToString());
            Name = name;
            if (UIResState == ResState.Loaded && LooksNode != null)
            {
                var nameboard = LooksNode.GetComponent<Nameboard>();
                nameboard.Name = name;
            }
        }

        public void UnuseLooks()
        {
            if (UIResState == ResState.Loaded && LooksNode != null)
            {
                LooksNode.gameObject.SetActive(false);
                ResMgr.GetInstance().UnuseGameObject("Nameboard", LooksNode.gameObject);
                UIResState = ResState.WaitLoad;
                LooksNode = null;
            }
        }
    }

}
