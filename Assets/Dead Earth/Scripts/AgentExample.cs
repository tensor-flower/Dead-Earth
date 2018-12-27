using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentExample : MonoBehaviour {
	//Config params
	[SerializeField] AIWaypointNetwork waypointNetwork = null;
	[SerializeField] int destPoint = 0;
	[SerializeField] NavMeshPathStatus pathStatus;

	//private variable
	private NavMeshAgent _navAgent = null;

	IEnumerator Start()
    {
		_navAgent = GetComponent<NavMeshAgent>();
		_navAgent.autoBraking = false;
		//GoToNextPoint();

        while (true)
        {
            if (_navAgent.isOnOffMeshLink)
            {
                yield return StartCoroutine(Jump(1.0f));
				_navAgent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

	void GoToNextPoint(){
		if(waypointNetwork.Waypoints.Count == 0)
			return;
		//TODO destPoint weird behaviour in inspector, increments even when destination not reached
		//Debug.Log(destPoint);
		_navAgent.destination = waypointNetwork.Waypoints[destPoint].position; 
		//Debug.Log("code reaches here");
		destPoint = (destPoint+1) % waypointNetwork.Waypoints.Count;
		//Debug.Log(destPoint);
	}
	
	void Update () {
		pathStatus = _navAgent.pathStatus;
		/* if(_navAgent.isOnOffMeshLink)
		{
			StartCoroutine(Jump(1.0f));
			//_navAgent.CompleteOffMeshLink();
		}*/
		
		//allows agent to move to closest point in partial path mode
		if (!_navAgent.pathPending && _navAgent.remainingDistance < _navAgent.stoppingDistance /* || pathStatus==NavMeshPathStatus.PathPartial*/)
            GoToNextPoint();
	}

	IEnumerator	Jump(float duration){
		OffMeshLinkData data = _navAgent.currentOffMeshLinkData;
        Vector3 startPos = _navAgent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * _navAgent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = _navAgent.height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            _navAgent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }	
	}
}
