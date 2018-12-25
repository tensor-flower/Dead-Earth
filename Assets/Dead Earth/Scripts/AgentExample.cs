using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentExample : MonoBehaviour {
	//Config params
	[SerializeField] AIWaypointNetwork waypointNetwork = null;
	[SerializeField] int destPoint = 0;

	//private variable
	private NavMeshAgent _navAgent = null;

	void Start () {
		_navAgent = GetComponent<NavMeshAgent>();
		_navAgent.autoBraking = false;
		GoToNextPoint();		
	}

	void GoToNextPoint(){
		if(waypointNetwork.Waypoints.Count == 0)
			return;
		_navAgent.destination = waypointNetwork.Waypoints[destPoint].position;
		destPoint = (destPoint+1) % waypointNetwork.Waypoints.Count;
	}
	
	void Update () {
		if (!_navAgent.pathPending && _navAgent.remainingDistance < 0.5f)
            GoToNextPoint();
	}

	
}
