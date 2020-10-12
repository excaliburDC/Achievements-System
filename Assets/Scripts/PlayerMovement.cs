using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask mask;

    private NavMeshAgent aiAgent;
    private Camera cam;
    
    

    // Start is called before the first frame update
    void Start()
    {
        aiAgent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                aiAgent.SetDestination(hit.point);
            }
        }

       
    }
}
