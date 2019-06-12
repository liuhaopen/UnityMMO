using UnityEngine;

using UnityEngine.AI;

public class ClickMoveTo : MonoBehaviour {
    private NavMeshAgent agent;

    void Start () {

            agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update () {
        RaycastHit hitInfo;
            agent = GetComponent<NavMeshAgent>();
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Input.mousePosition : "+Input.mousePosition.ToString()+" Camera.current:"+(Camera.main!=null).ToString());
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo)) {
                Debug.Log("hit : "+hitInfo.point.ToString());
                agent.SetDestination(hitInfo.point);
            }
        }
    }
}