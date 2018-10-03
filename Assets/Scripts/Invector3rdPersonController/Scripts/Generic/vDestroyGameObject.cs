using UnityEngine;
using System.Collections;

public class vDestroyGameObject : MonoBehaviour
{
    public float delay;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
