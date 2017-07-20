using UnityEngine;
using System.Collections;
using Spine.Unity;

public class PlayerAnimator : MonoBehaviour {

	[SpineAnimation] public string idle;
	public float idleTimeScale = 0.75F;
	[SpineAnimation] public string walk;
	public float walkTimeScale = 2;
	[SpineAnimation] public string carry;
	public bool facingLeft;

	private SkeletonAnimation skeletonAnimation;
	private bool walking;
	private PlayerMovement playerMovement;

	void Start() {
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		playerMovement = GameManager.instance.playerMovement;
		skeletonAnimation.timeScale = idleTimeScale;
		skeletonAnimation.state.SetAnimation(0, idle, true);
	}

	void LateUpdate() {
		if (PauseMenu.paused || !GameManager.instance.playerController.controllable)
			return;

		Vector3 inputVector = playerMovement.GetInputVector();

		// Face character in the correct direction
		if (inputVector.x > 0)
			skeletonAnimation.skeleton.flipX = facingLeft;
		else if (inputVector.x < 0)
			skeletonAnimation.skeleton.flipX = !facingLeft;

		UpdateAnimation(inputVector);
	}

	private void UpdateAnimation(Vector3 inputVector) {
		// Select idle or walking animations
		float inputMagnitude = playerMovement.GetInputMagnitude();
		bool walking = inputMagnitude != 0;
		if (walking) {
			if (!this.walking) {
				skeletonAnimation.state.SetAnimation(0, walk, true);
			}
			skeletonAnimation.timeScale = walkTimeScale * inputMagnitude;
		} else {
			if (this.walking) {
				skeletonAnimation.state.SetAnimation(0, idle, true);
			}
			skeletonAnimation.timeScale = idleTimeScale;
		}
		this.walking = walking;
	}

	public void Idle() {
		skeletonAnimation.state.SetAnimation(0, idle, true);
		skeletonAnimation.timeScale = idleTimeScale;
	}
}
