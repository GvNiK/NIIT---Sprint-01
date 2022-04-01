using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardPatrolling : MonoBehaviour    //Assignment - 03
{
    private NavMeshAgent meshAgent;
    public List<Transform> wayPoints;
    private int wayPointIndex;

    // Start is called before the first frame update
    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        wayPointIndex = 0;
        meshAgent.SetDestination(wayPoints[wayPointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        //'!meshAgent.hasPath' - Checks if the Enemy has No Path, i.e, if its reached a waypoint, set its path to 'No Path' & execute the code.
        // 1 Unity Unit = 1 Meter.
        //'GetDistanceFromDestination() < 0.2f' - Sets a condition that if the the Enemy reaches a Distance around 1 meter or 1 Unity Unit, 
        //then move towards next waypoint, i.e., execute the following code.
        if(!meshAgent.hasPath || GetDistanceFromDestination() < 0.2f)    
        {
            wayPointIndex++;

            if(wayPointIndex >= wayPoints.Count)
            {
                //wayPointIndex--;
                //meshAgent.SetDestination(wayPoints[wayPointIndex].position);
                //return;

                wayPointIndex = 0;

            }
            meshAgent.SetDestination(wayPoints[wayPointIndex].position);
        }
    }
    private float GetDistanceFromDestination()  //Calculates the Distance between a Waypoint and the Enemy
    {
        return (meshAgent.destination - meshAgent.gameObject.transform.position).magnitude; 
    }
}
