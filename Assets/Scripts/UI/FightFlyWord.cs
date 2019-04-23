using UnityEngine;

namespace UnityMMO
{
public class FightFlyWord : MonoBehaviour {
    float endTime = 0;
    private void Awake() 
    {
        
    }

    private void Update() 
    {
        // transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
        if (endTime - Time.time <= 0)
        {
            Object.Destroy(gameObject, 0.1f);
        }
    }

    public void SetData(long num, long flag)
    {

    }

    public void StartFly()
    {
        endTime = Time.time + 1;
    }
}

}