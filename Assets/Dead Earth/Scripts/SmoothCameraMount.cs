using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraMount : MonoBehaviour {

	[SerializeField] Transform cameraPos;
	[SerializeField] float _speed = 3f;

	void Start () {
		
	}
	
	void LateUpdate () {
		transform.position = Vector3.Lerp(transform.position, cameraPos.position, Time.deltaTime * _speed);
		transform.rotation = Quaternion.Slerp(transform.rotation, cameraPos.rotation, Time.deltaTime * _speed);
	}
}
