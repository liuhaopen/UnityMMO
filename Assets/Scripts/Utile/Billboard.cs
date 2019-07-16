using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 target = transform.position+Camera.main.transform.rotation*Vector3.back;
        // Vector3 up = Camera.main.transform.rotation*Vector3.up;
        // transform.LookAt(target, up);
        transform.LookAt(Camera.main.transform.position);
    }
}
