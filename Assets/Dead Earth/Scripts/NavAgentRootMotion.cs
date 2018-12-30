using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentRootMotion : MonoBehaviour {
	//Config params
	[SerializeField] AIWaypointNetwork waypointNetwork = null;
	[SerializeField] int destPoint = 0;
	[SerializeField] NavMeshPathStatus pathStatus;
	[SerializeField] bool mixedMode = true;

	//private variable
	private NavMeshAgent _navAgent = null;
	private Animator _animator = null;
	private float _smoothAngle;
	private Quaternion _lookRotation;

	void Start()
    {
		_navAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		//_navAgent.autoBraking = false;
		_navAgent.updateRotation = false;
		GoToNextPoint();
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

	void OnAnimatorMove(){
		if(mixedMode && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
		{
			transform.rotation = _animator.rootRotation;
		}
		//agent velocity is set by animator root motion
		_navAgent.velocity = _animator.deltaPosition / Time.deltaTime;
	}
	
	void Update () {
		pathStatus = _navAgent.pathStatus;

		Vector3 desiredVelocity = _navAgent.desiredVelocity;
		Vector3 localDesiredVelocity = transform.InverseTransformVector(desiredVelocity);
		float angle = Mathf.Atan2(localDesiredVelocity.x, localDesiredVelocity.z) * Mathf.Rad2Deg;
		_smoothAngle = Mathf.MoveTowardsAngle(_smoothAngle, angle, 80f*Time.deltaTime);

		_animator.SetFloat("Angle", _smoothAngle);
		_animator.SetFloat("Speed", localDesiredVelocity.z, 0.1f, Time.deltaTime);

		if(!mixedMode || mixedMode && _animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion") && Mathf.Abs(angle)<80f)
			UpdateRotation(desiredVelocity);

		//allows agent to move to closest point in partial path mode
		if (!_navAgent.pathPending && _navAgent.remainingDistance < _navAgent.stoppingDistance /* || pathStatus==NavMeshPathStatus.PathPartial*/)
            GoToNextPoint();
	}

	void UpdateRotation(Vector3 desiredVelocity){
		if(desiredVelocity.sqrMagnitude>Mathf.Epsilon)
		{
			//always look in direction of desired velocity
			_lookRotation = Quaternion.LookRotation(desiredVelocity, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 5f*Time.deltaTime);
		}
	}

}
