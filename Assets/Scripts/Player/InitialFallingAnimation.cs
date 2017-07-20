using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class InitialFallingAnimation : MonoBehaviour {

	[SpineAnimation] public string fallingAnim;

	private CharacterController characterController;

	void Start() {
		characterController = GetComponentInParent<CharacterController>();
		GetComponent<SkeletonAnimation>().state.SetAnimation(0, fallingAnim, true);
	}
	
	void Update() {
		if (characterController.isGrounded) {
			GetComponent<PlayerAnimator>().enabled = true;
			Destroy(this);
		}
	}
}
