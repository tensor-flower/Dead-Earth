using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentNoRootMotion : MonoBehaviour {
	//Config params
	[SerializeField] AIWaypointNetwork waypointNetwork = null;
	[SerializeField] int destPoint = 0;
	[SerializeField] NavMeshPathStatus pathStatus;

	//private variable
	private NavMeshAgent _navAgent = null;
	private Animator _animator = null;
	private int _turnOnSpot;
	private float _originalMaxSpeed;

	IEnumerator Start()
    {
		_navAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_originalMaxSpeed = _navAgent.speed;
		_navAgent.autoBraking = false;
		GoToNextPoint();

        /* while (true)
        {
            if (_navAgent.isOnOffMeshLink)
            {
                yield return StartCoroutine(Jump(1.0f));
				_navAgent.CompleteOffMeshLink();
            }
            yield return null;
		}*/
		yield return null;
    }

	void GoToNextPoint(){
		if(waypointNetwork.Waypoints.Count == 0)
			return;
		//TODO destPoint weird behaviour in inspector, increments even when destination not reached
		//Debug.Log(destPoint);
		_navAgent.destination = waypointNetwork.Waypoints[destPoint].position; 
		destPoint = (destPoint+1) % waypointNetwork.Waypoints.Count;
		//Debug.Log(destPoint);
	}
	
	void Update () {
		pathStatus = _navAgent.pathStatus;
		 
		Vector3 crossProduct = Vector3.Cross(transform.forward, _navAgent.desiredVelocity.normalized);
		float horizontal = (crossProduct.y>0) ? crossProduct.magnitude : -crossProduct.magnitude;
		horizontal = Mathf.Clamp(horizontal * 4.31f, -2.31f, 2.31f);
		float vertical = _navAgent.desiredVelocity.magnitude;
		//Debug.Log("horizontal: "+horizontal+", Vertical: "+vertical);
		Debug.Log(vertical);
		
		if(vertical<1f && Vector3.Angle(transform.forward, _navAgent.desiredVelocity)>10f){
			Debug.Log("here");
			_navAgent.speed = 0.1f;
			_turnOnSpot = (int)Mathf.Sign(horizontal);
		}else{
			_navAgent.speed = _originalMaxSpeed;
			_turnOnSpot = 0;
		}
		
		
		_animator.SetFloat("Horizontal", horizontal);
		_animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
		
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
