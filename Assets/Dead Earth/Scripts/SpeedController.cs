using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour {

	Animator animator;
	private int _horizontalHash;
	private int _verticalHash;
	private int _attackHash;

	void Start () {
		animator = GetComponent<Animator>();
		_horizontalHash = Animator.StringToHash("Horizontal");
		_verticalHash = Animator.StringToHash("Vertical");
		_attackHash = Animator.StringToHash("Attack");
	}
	
	void Update () {
		float horizontalThrow = Input.GetAxis("Horizontal") * 2.3f;
		float verticalThrow = Input.GetAxis("Vertical") * 5.66f;
		//Debug.Log(horizontalThrow + ", " + verticalThrow);
		animator.SetFloat(_horizontalHash, horizontalThrow, 0.1f, Time.deltaTime);
		animator.SetFloat(_verticalHash, verticalThrow, 1f, Time.deltaTime);
		if(Input.GetKeyDown("space"))
			animator.SetTrigger(_attackHash);
	}
}
