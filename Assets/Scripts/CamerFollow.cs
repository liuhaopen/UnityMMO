using UnityEngine;
using System.Collections;

public class CamerFollow : MonoBehaviour {
	public Transform target;
    [SerializeField]
	public float distanceH = 23.5f;
    [SerializeField]
	public float distanceV = 13.3f;
	public float lookAtOffsetY = 3f;
	static float rotate = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate()
	{
		Vector3 nextpos = Vector3.forward * -1 * distanceH + Vector3.up * distanceV + target.position;
		this.transform.position = nextpos;
//		this.transform.LookAt(target);
	}
}
