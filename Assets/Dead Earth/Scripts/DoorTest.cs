using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState{Open, Animating, Closed};

public class DoorTest : MonoBehaviour {

	public float SlidingDistance = 4.5f;
	public float Duration = 1.5f;
	public AnimationCurve Curve = new AnimationCurve();

	private Vector3 _closedPos;
	private Vector3 _openPos;
	[SerializeField] DoorState _doorState = DoorState.Closed;

	void Start () {
		_closedPos = transform.position;
		_openPos = _closedPos - transform.right * SlidingDistance;
	}
	
	void Update () {
		if(Input.GetKeyDown("space") && _doorState!=DoorState.Animating){
			StartCoroutine(AnimateDoor(_doorState));
		}
	}

	IEnumerator AnimateDoor(DoorState doorState){
		_doorState = DoorState.Animating;
		//Debug.Log(doorState);
		Vector3 startPos = (doorState==DoorState.Closed) ? _closedPos : _openPos;
		Vector3 endPos = (doorState==DoorState.Closed) ? _openPos : _closedPos;

		float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, Curve.Evaluate(normalizedTime));
            normalizedTime += Time.deltaTime / Duration;
            yield return null;
        }
		_doorState = (doorState==DoorState.Closed) ? DoorState.Open : DoorState.Closed;
	}
}
