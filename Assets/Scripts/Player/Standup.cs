using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Standup : MonoBehaviour {

	[SpineAnimation] public string sceneStart;

	private SkeletonAnimation skeletonAnimation;

	void Start () {
		GameManager.instance.playerController.SetControllable(false);
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		skeletonAnimation.state.SetAnimation(0, sceneStart, false);
		skeletonAnimation.state.Complete += OnAnimEnd;
	}

	private void OnAnimEnd(Spine.TrackEntry trackEntry) {
		GameManager.instance.playerController.SetControllable(true);
		GetComponent<PlayerAnimator>().enabled = true;
		skeletonAnimation.state.Complete -= OnAnimEnd;
		Destroy(this);
	}
}
