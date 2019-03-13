using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TestCharacterController : MonoBehaviour
{
    public Transform FreeLookCameraTrans;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        var cameraObj = GameObject.Find("FreeLookCamera");
        FreeLookCameraTrans = cameraObj.transform;

        cc = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2();
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        var forward = FreeLookCameraTrans.TransformDirection(Vector3.forward);
        forward.y = 0;
        var right = FreeLookCameraTrans.TransformDirection(Vector3.right);
        float3 targetDirection = input.x * right + input.y * forward;
        targetDirection.y = -10;
        Debug.Log("targetDirection : "+targetDirection.x+" "+targetDirection.y+" "+targetDirection.z);
        cc.Move(targetDirection*Time.deltaTime);
    }
}
